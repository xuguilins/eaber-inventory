namespace LCPC.Domain.Commands;

public class DeleteExtraCommand:IRequest<ReturnResult>
{
    public string[] Ids  { get; private set; }

    public void AddIds(string[] ids)
    {
        this.Ids = ids;
    }
}