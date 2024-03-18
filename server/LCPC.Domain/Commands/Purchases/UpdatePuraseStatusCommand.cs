namespace LCPC.Domain.Commands;

public class UpdatePuraseStatusCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }
    public InOStatus InOStatus { get; set; }

}