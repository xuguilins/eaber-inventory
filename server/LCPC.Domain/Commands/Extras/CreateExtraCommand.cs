namespace LCPC.Domain.Commands;

public class CreateExtraCommand:IRequest<ReturnResult>
{
    public decimal Price { get; set; }
    public ExtraType ExtraType { get; set; }
    public string TypeName { get; set; }
    public string Remark { get; set; }
}