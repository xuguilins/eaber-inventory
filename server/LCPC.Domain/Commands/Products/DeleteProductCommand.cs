namespace LCPC.Domain.Commands;

public class DeleteProductCommand:IRequest<ReturnResult>
{
    public string[] Ids { get;private  set; }

    public void AddIds(string[] _ids)
    {
        Ids = _ids;
    }
    
}