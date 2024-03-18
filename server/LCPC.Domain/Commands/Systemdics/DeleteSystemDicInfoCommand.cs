namespace LCPC.Domain.Commands;

public class DeleteSystemDicInfoCommand:IRequest<ReturnResult>
{
    public string[] Ids { get; private set; }

    public void AddIds(string[] ids)
    {
        this.Ids = ids;
    }
}