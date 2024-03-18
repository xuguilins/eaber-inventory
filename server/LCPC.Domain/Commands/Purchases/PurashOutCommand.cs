namespace LCPC.Domain.Commands;

public class PurashOutCommand:IRequest<ReturnResult>
{
    public string InOrderCode { get; set; }
    public string Reason { get; set; }
    public string InPhone { get; set; }
    public string OutOrderTime { get; set; }
    public string InUser { get; set; }
    public string Logics { get; set; }
    public string SupilerId { get; set; }
    public List<PurashOutDetail> Detail { get; set; } = new List<PurashOutDetail>();
}

public record PurashOutDetail
{
    public decimal InPrice { get; set; }
    public decimal OutPrice { get; set; }
    public string ProductCode { get; set; }
    public int ProductCount { get; set; }
    public int ReturnCount { get; set; }
    public string ProductModel { get; set; }
    public string ProductName { get; set; }
   
}
