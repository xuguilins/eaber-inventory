using System.ComponentModel;

namespace LCPC.Domain.Entities;

public enum RuleType
{
    [Description("产品编码")]
    Product = 1,
    [Description("订单编码")]
    Order = 2,
    [Description("供应商编码")]
    supplier = 3,
    [Description("进货单编码")]
    PurchaseIn = 4,
    [Description("退货单编码")]
    PurchaseOut = 5,
    [Description("客户编码")]
    Customer = 6,
    [Description("其它收入/支出编码")]
    ExtraOrder = 7 
}