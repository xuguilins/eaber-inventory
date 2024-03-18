namespace LCPC.Domain.Commands;

public class UpdatePurashOutCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }
    public string InOrderCode { get; set; }
    public string Reason { get; set; }
    public string InPhone { get; set; }
    public string OutOrderTime { get; set; }
    public string InUser { get; set; }
    public string Logics { get; set; }
    public string SupilerId { get; set; }
    public List<PurashOutDetail> Detail { get; set; } = new List<PurashOutDetail>();
}