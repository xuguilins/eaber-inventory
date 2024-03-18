using Dapper;

namespace LCPC.Domain.CommandHandlers;

public class DeletePurashOutCommandHandler:IRequestHandler<DeletePurashOutCommand,ReturnResult>
{
    private readonly ISqlDapper _sqlDapper;
    private readonly IPurchaseOutOrderRepository _orderRepository;
    private readonly IPurchaseOutOrderDetailRepository _orderDetailRepository;
    private readonly IProdcutRepository _prodcutRepository;
    public DeletePurashOutCommandHandler(ISqlDapper sqlDapper,
        IPurchaseOutOrderRepository purchaseOutOrderRepository,
        IPurchaseOutOrderDetailRepository detailRepository,
        IProdcutRepository prodcutRepository
        )
    {
        _orderRepository = purchaseOutOrderRepository;
        _orderDetailRepository = detailRepository;
        _sqlDapper = sqlDapper;
        _prodcutRepository = prodcutRepository;

    }
    public async  Task<ReturnResult> Handle(DeletePurashOutCommand request, CancellationToken cancellationToken)
    {
        var list = await _orderRepository.GetEntitiesAsync(d => request.Ids.Contains(d.Id));
        var detals = await _orderDetailRepository.GetEntitiesAsync(d => request.Ids.Contains(d.PurchaseId));
        //删除子表
        var mainids = list.Select(d => d.Id);
        var childs = detals.Select(d => d.Id);
        var number = detals.Select(d => d.ProductCode).ToList();
        var products = await _prodcutRepository.GetEntitiesAsync(d => number.Contains(d.ProductCode));
        int result = await _sqlDapper.OpenConnectionAsync(con =>
        {
            //删除子表
            var transction = con.BeginTransaction();
            con.Execute("delete from  PurchaseOutOrderDetail where Id in  @Id", new { Id = childs }, transction);
            //删除主表
            con.Execute("delete from  PurchaseOutOrder where Id in @Id", new { Id = mainids }, transction);
            //更新库存
            detals.ForEach(item =>
            {
                var model = products.FirstOrDefault(d => d.ProductCode == item.ProductCode);
                if (model != null)
                {
                    int value = model.InventoryCount + item.OutCount;
                    con.Execute("update ProductInfo set InventoryCount=@InventoryCount where Id=@Id",
                        new { InventoryCount = value, Id = model.Id }, transction);
                }
            });
            transction.Commit();
            return list.Count;
        });
        return result > 0
            ? new ReturnResult(true, null, MessageHelper.DeleteMessage(list.Count))
            : new ReturnResult(false, null, MessageHelper.DeleteMessage(list.Count, false));
    }
}