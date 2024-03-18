namespace LCPC.Domain.Commands;

public class UpdateCustomerStatusCommand:IRequest<ReturnResult>
{
    public string Id { get; private set; }

    public void AddId(string id)
    {
        this.Id = id;
    }
}