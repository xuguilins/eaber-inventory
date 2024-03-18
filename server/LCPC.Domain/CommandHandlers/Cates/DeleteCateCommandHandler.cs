using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class DeleteCateCommandHandler : IRequestHandler<DeleteCateCommand, ReturnResult>
    {
        private readonly ICatetoryRepository _cateRepository;
        public DeleteCateCommandHandler( ICatetoryRepository catetoryRepository) {
            _cateRepository = catetoryRepository;
         }
        public async Task<ReturnResult> Handle(DeleteCateCommand request, CancellationToken cancellationToken)
        {
            var list = await _cateRepository.GetEntitiesAsync(x=> request.Ids.Any(v=>v == x.Id));
            await _cateRepository.RemoveAsync(list);
            int result = await _cateRepository.UnitOfWork.SaveChangesAsync();
            return result >0 ? new ReturnResult(true,null,MessageHelper.DeleteMessage(list.Count))
             : new ReturnResult(false,null,MessageHelper.DeleteMessage(list.Count,false));
        }
    }
}