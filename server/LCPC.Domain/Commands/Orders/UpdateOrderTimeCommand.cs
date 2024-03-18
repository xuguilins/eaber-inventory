namespace LCPC.Domain.Commands;

public class UpdateOrderCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }
    public string PayName { get; set; }
    public string Time { get; set; }
}