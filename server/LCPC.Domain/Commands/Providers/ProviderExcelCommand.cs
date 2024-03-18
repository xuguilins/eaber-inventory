using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands.Providers
{
    public class ProviderExcelCommand<T>:IRequest<ReturnResult>,IExcelCommand
    where T:class,new()
    {
        public List<T> Providers {get;set;}
    }
}