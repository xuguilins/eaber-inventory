using Dapper;
using LCPC.Domain.EventHandlers.EventDatas;
using LCPC.Share;

namespace LCPC.Domain.CommandHandlers;

public class UpdateUpdatePurashOutCommandHandler:IRequestHandler<UpdatePurashOutCommand,ReturnResult>
{
    private readonly IPurchaseOutOrderRepository _purchaseOutOrderRepository;
    private readonly IPurchaseOutOrderDetailRepository _orderDetailRepository;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
    private readonly UserHelper _userHelper;
    private readonly IProdcutRepository _prodcutRepository;
    private readonly ISqlDapper _sqlDapper;
    public UpdateUpdatePurashOutCommandHandler(IPurchaseOutOrderRepository purchaseOutOrderRepository,
        ISupilerInfoRepository  supilerInfoRepository,
        UserHelper userHelper,
        IRuleManager ruleManager,
        IProdcutRepository prodcutRepository,
        IMediator mediator,
        IPurchaseOutOrderDetailRepository purchaseOutOrderDetailRepository,
        ISqlDapper sqlDapper
    )
    {
        _purchaseOutOrderRepository = purchaseOutOrderRepository;
        _supilerInfoRepository = supilerInfoRepository;
        _userHelper = userHelper;
        _prodcutRepository = prodcutRepository;
        _orderDetailRepository = purchaseOutOrderDetailRepository;
        _sqlDapper = sqlDapper;
    }
    public async Task<ReturnResult> Handle(UpdatePurashOutCommand request, CancellationToken cancellationToken)
    {
      var user = _userHelper.LoginName;
        var order = await _purchaseOutOrderRepository.GetByKey(request.Id);
        if (order == null)
            throw new Exception($"无效的退货单");
        var supiler = await _supilerInfoRepository.GetByKey(request.SupilerId);
        if (supiler == null)
            throw new Exception("进货单据内的供应商异常");
        var details = await _orderDetailRepository.GetEntitiesAsync(d => d.PurchaseId.Equals(order.Id));
        var numbers = details.Select(d => d.ProductCode);
        var products = await _prodcutRepository.GetEntitiesAsync(d => numbers.Contains(d.ProductCode));
        string mainSql = @"update PurchaseOutOrder set OrderTime=@OrderTime,
                            InOrderCode=@InOrderCode, Logicse=@Logicse, InUser=@InUser,
                            InPhone=@InPhone, Remark=@Remark, SupilerId=@SupilerId,OutOrderCount =@OutOrderCount  where Id=@Id";

        int result = await _sqlDapper.OpenConnectionAsync(con =>
        {
            int count = 0;
            //获取退货总数
            if (request.Detail.Any())
                 count = request.Detail.Sum(d => d.ReturnCount);
            var transaction = con.BeginTransaction();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OrderTime",request.OutOrderTime);
            parameters.Add("@OutOrderCount",count);
            parameters.Add("@InOrderCode",request.InOrderCode);
            parameters.Add("@Logicse",request.Logics);
            parameters.Add("@InUser",request.InUser);
            parameters.Add("@InPhone",request.InPhone);
            parameters.Add("@Remark",request.Reason);
            parameters.Add("@SupilerId",request.SupilerId);
            parameters.Add("@Id",request.Id);
            con.Execute(mainSql, parameters, transaction);
            //移除退货单
            string deleteSql = @"delete from  PurchaseOutOrderDetail where PurchaseId=@PurchaseId";
            con.Execute(deleteSql, new { PurchaseId = order.Id },transaction);

            #region 还原库存

            
            List<ProductInfo> newProduct = new List<ProductInfo>();
            details.ForEach(item =>
            {
                var product = products.FirstOrDefault(d => d.ProductCode == item.ProductCode);
                if (product != null)
                {
                    product.InventoryCount += item.OutCount;
                    newProduct.Add(product);
                    // con.Execute("update ProductInfo set InventoryCount=@InventoryCount where Id=@Id",new { InventoryCount =  value, })
                }
            });
            #endregion

              #region 写入明细
            string detailSql =
                @"insert into PurchaseOutOrderDetail(id, productcode, productname, productmodel, incount, inprice, outcount, outprice, outallprice, purchaseid, createtime, createuser, remark, enable)
values (@id, @productcode, @productname, @productmodel, @incount, @inprice, @outcount, @outprice, @outallprice, @purchaseid, @createtime, @createuser, @remark, @enable)";
            foreach (var product in request.Detail)
            {
                var money = (product.ReturnCount) * product.OutPrice;
                var detailId = UtilHelper.getNewId();
                var item = products.FirstOrDefault(d => d.ProductCode == product.ProductCode);
                if (item != null)
                {
                    #region 写入子表

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
                    dhs.Add("@purchaseid",order.Id);
                    dhs.Add("@createtime",DateTime.Now);
                    dhs.Add("@createuser",user);
                    dhs.Add("@remark","");
                    dhs.Add("@enable",true);
                    #endregion
                    con.Execute(detailSql, dhs,transaction);
                }
               
            }
            #endregion

             #region 更新实际库存

            request.Detail.ForEach(item =>
            {
                var product = newProduct.FirstOrDefault(d => d.ProductCode == item.ProductCode);
                if (product != null)
                {
                    int value = product
                        .InventoryCount - item.ReturnCount;
                    con.Execute("update ProductInfo set InventoryCount=@InventoryCount where Id=@Id",
                        new { InventoryCount = value, Id = product.Id }, transaction);
                }
            });

            #endregion
            transaction.Commit();
            return 1;
        });
    
        return result > 0
            ? new ReturnResult(true, null, "退货单更新成功")
            : new ReturnResult(false, null, "退货单更新失败");
    }
}