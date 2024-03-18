namespace LCPC.Domain.Commands;

public class UpdatePurashOutStatusCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }
    public OutStatus OutStatus { get; set; }
}