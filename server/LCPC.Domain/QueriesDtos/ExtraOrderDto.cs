namespace LCPC.Domain.QueriesDtos;

public class ExtraSearch : DataSearch
{
    public ExtraType ExtraType { get; set; }
}
public class ExtraOrderDto
{
    public string Id { get; set; }
    public string TypeName { get; set; }
    public string Remark { get; set; }
    public decimal Price { get; set; }
    public bool Enable { get; set; }
    public string OrderCode { get; set; }
    public ExtraType ExtraType { get; set; }
}