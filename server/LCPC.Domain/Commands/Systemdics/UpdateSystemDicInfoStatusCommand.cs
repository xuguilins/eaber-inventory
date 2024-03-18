namespace LCPC.Domain.Commands;

public class UpdateSystemDicInfoStatusCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }

    public void AddId(string id)
    {
        this.Id = id;
    }
}