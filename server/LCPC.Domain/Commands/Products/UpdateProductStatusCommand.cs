namespace LCPC.Domain.Commands;

public class UpdateProductStatusCommand:IRequest<ReturnResult>
{
    public string Id { get; private set; }

    public void AddId(string id)
    {
        Id = id;
    }
}