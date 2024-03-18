using TinyPinyin;

namespace LCPC.Domain.CommandHandlers;

public class UpdateCustomerCommandHandler:IRequestHandler<UpdateCustomerCommand,ReturnResult>
{
    private readonly ICustomerInfoRepository _customerInfoRepository;
    public UpdateCustomerCommandHandler(ICustomerInfoRepository customerInfoRepository)
    {
        _customerInfoRepository = customerInfoRepository;
    }
    public async Task<ReturnResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var model = await _customerInfoRepository.FindEntity(d=>d.Id.Equals(request.Id));
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.Enable = request.Enable;
        model.CustomerUser = request.CustomerUser;
        model.CustomerName = request.CustomerName;
        model.NameSpell = PinyinHelper.GetPinyinInitials(model.CustomerName);
        model.Address = request.Address;
        model.TelNumber = request.TelNumber;
        model.PhoneNumber = request.PhoneNumber;
        model.Remark = request.Remark;
        await _customerInfoRepository.UpdateAsync(model);
        int result = await _customerInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "更新成功")
            : new ReturnResult(false, null, "更新失败");
    }
}