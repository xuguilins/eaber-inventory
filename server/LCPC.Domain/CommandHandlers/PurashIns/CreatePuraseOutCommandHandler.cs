using System.Collections;
using Dapper;
using LCPC.Domain.EventHandlers.EventDatas;
using LCPC.Share;
using Microsoft.Data.SqlClient;

namespace LCPC.Domain.CommandHandlers;

public class CreatePuraseOutCommandHandler:IRequestHandler<PurashOutCommand,ReturnResult>
{
    private readonly IPurchaseOutOrderRepository _purchaseOutOrderRepository;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
    private readonly UserHelper _userHelper;
    private readonly IRuleManager _ruleManager;
    private readonly IProdcutRepository _prodcutRepository;
    private readonly ISqlDapper _sqlDapper;
    public CreatePuraseOutCommandHandler(IPurchaseOutOrderRepository purchaseOutOrderRepository,
        ISupilerInfoRepository  supilerInfoRepository,
        UserHelper userHelper,
        IRuleManager ruleManager,
        IProdcutRepository prodcutRepository,
        ISqlDapper sqlDapper
    )
    {
        _purchaseOutOrderRepository = purchaseOutOrderRepository;
        _supilerInfoRepository = supilerInfoRepository;
        _userHelper = userHelper;
        _prodcutRepository = prodcutRepository;
        _ruleManager = ruleManager;
        _sqlDapper = sqlDapper;
    }
    public async Task<ReturnResult> Handle(PurashOutCommand request, CancellationToken cancellationToken)
    {
        
        var user = _userHelper.LoginName;
        var order = await _purchaseOutOrderRepository.FindEntity(d => d.InOrderCode.Equals(request.InOrderCode));
        if (order != null)
            throw new Exception($"此进货单[{request.InOrderCode}]已创建过退货单");
        var supiler = await _supilerInfoRepository.GetByKey(request.SupilerId);
        if (supiler == null)
            throw new Exception("进货单据内的供应商异常");
        var code = await _ruleManager.getNextRuleNumber(RuleType.PurchaseOut);
        var numbers = request.Detail.Select(d => d.ProductCode);
        var products =  await _prodcutRepository.GetEntitiesAsync(d => numbers.Contains(d.ProductCode));
        int result =await _sqlDapper.OpenConnectionAsync<int>(con =>
        {
          //  con.Open();
            var tarnsaction = con.BeginTransaction();
            string insertSql = @"
insert into PurchaseOutOrder(id, purchasecode, ordertime, inordercode, supilerid, inuser, logicse, outorderprice, inphone, outstatus, createtime, createuser, remark, enable, outordercount) 
values(@id, @purchasecode, @ordertime, @inordercode, @supilerid, @inuser, @logicse, @outorderprice, @inphone, @outstatus, @createtime, @createuser, @remark, @enable, @outordercount)";
            var id = UtilHelper.getNewId();
           
            #region 写入主表
            DynamicParameters hs = new DynamicParameters();
            hs.Add("@id",id);
            hs.Add("@purchasecode",code);
            hs.Add("@ordertime",request.OutOrderTime);
            hs.Add("@inordercode",request.InOrderCode);
            hs.Add("@supilerid",supiler.Id);
            hs.Add("@inuser",request.InUser);
            hs.Add("@logicse",request.Logics);
            hs.Add("@inphone",request.InPhone);
            hs.Add("@outstatus",OutStatus.INING);
            hs.Add("@createtime",DateTime.Now);
            hs.Add("@createuser",user);
            hs.Add("@remark",request.Reason);
            hs.Add("@enable",true);
            var allCount = request.Detail.Sum(d => d.ReturnCount);
            decimal allPrice = 0.00M;
            request.Detail.ForEach(item =>
            {
                var money = (item.ReturnCount) * item.OutPrice;
                allPrice += money;
            });
            hs.Add("@outordercount",allCount);
            hs.Add("@outorderprice",allPrice);
            #endregion

            con.Execute(insertSql, hs,tarnsaction);
            string detailSql =
                @"insert into PurchaseOutOrderDetail(id, productcode, productname, productmodel, incount, inprice, outcount, outprice, outallprice, purchaseid, createtime, createuser, remark, enable)values (@id, @productcode, @productname, @productmodel, @incount, @inprice, @outcount, @outprice, @outallprice, @purchaseid, @createtime, @createuser, @remark, @enable)";

    
            foreach (var product in request.Detail)
            {
                var detailId = UtilHelper.getNewId();
                var item = products.FirstOrDefault(d => d.ProductCode == product.ProductCode);
                if (item != null)
                {
                    #region 写入子表

                    var money = (product.ReturnCount) * product.OutPrice;
                    DynamicParameters dhs = new DynamicParameters();
                  
                    dhs.Add("@id",detailId);
                    dhs.Add("@productcode",item.ProductCode);
                    dhs.Add("@productname",item.ProductName);
                    dhs.Add("@productmodel",item.ProductModel);
                    dhs.Add("@incount",product.ProductCount);
                    dhs.Add("@inprice",product.InPrice);
                    dhs.Add("@outcount",product.ReturnCount);
                    dhs.Add("@outprice",product.OutPrice);
                    dhs.Add("@outallprice",money);
                    dhs.Add("@purchaseid",id);
                    dhs.Add("@createtime",DateTime.Now);
                    dhs.Add("@createuser",user);
                    dhs.Add("@remark","");
                    dhs.Add("@enable",true);
                    allCount += product.ReturnCount;
                    allPrice += money;
                    #endregion
            
                    con.Execute(detailSql, dhs,tarnsaction);
                }
               
            }

            

            #region 扣减库存

            foreach (var product in request.Detail)
            {
                var item = products.FirstOrDefault(d => d.ProductCode == product.ProductCode);
                if (item != null)
                {
                    int value = item.InventoryCount - product.ReturnCount;
                    con.Execute("update ProductInfo set InventoryCount=@InventoryCount where Id=@Id",
                        new { Id = item.Id, InventoryCount = value },tarnsaction);
                }
               
            }


            #endregion
            
            tarnsaction.Commit();
            return 1;

        });

         return result > 0
            ? new ReturnResult(true, null, "退货单创建成功")
             : new ReturnResult(false, null, "退货单创建失败");
    }
}