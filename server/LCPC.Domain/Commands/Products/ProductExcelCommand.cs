namespace LCPC.Domain.Commands;

public class ProductExcelCommand<T>:IRequest<ReturnResult>,IExcelCommand
    where T:class,new()
{
    public List<T> Products {get;set;}
}