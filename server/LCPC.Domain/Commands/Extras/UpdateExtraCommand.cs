namespace LCPC.Domain.Commands;

public class UpdateExtraCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }
    public decimal Price { get; set; }
    public ExtraType ExtraType { get; set; }
    public string TypeName { get; set; }
    public string Remark { get; set; }
}