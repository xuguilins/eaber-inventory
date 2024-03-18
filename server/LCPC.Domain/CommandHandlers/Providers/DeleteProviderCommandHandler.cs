using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LCPC.Domain.Commands;
using LCPC.Domain.IRepositories;
using LCPC.Share.Response;
using MediatR;

namespace LCPC.Domain.CommandHandlers
{
    public class DeleteProviderCommandHandler : IRequestHandler<DeleteProviderCommand, ReturnResult>
    {
        private readonly ISupilerInfoRepository _supilerInfoRepository;
        public DeleteProviderCommandHandler(ISupilerInfoRepository supilerInfoRepository) {
            _supilerInfoRepository = supilerInfoRepository;
        }
        public  async  Task<ReturnResult> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
        {
             var list =  await _supilerInfoRepository.GetEntitiesAsync(x=> request.Ids.Contains(x.Id) );
             await _supilerInfoRepository.RemoveAsync(list);
            int result = await _supilerInfoRepository.UnitOfWork.SaveChangesAsync();
            return result>0? new ReturnResult(true,null,MessageHelper.DeleteMessage(list.Count))
            :new ReturnResult(false,null,MessageHelper.DeleteMessage(list.Count,false));
        }
    }
}