namespace LCPC.Domain.Commands;

public class CateExcelCommand<T>:IRequest<ReturnResult>,IExcelCommand
    where T:class,new()
{
    public List<T> Cates {get;set;}
}