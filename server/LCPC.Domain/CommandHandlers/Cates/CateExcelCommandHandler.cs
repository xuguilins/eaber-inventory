namespace LCPC.Domain.CommandHandlers.Cates;


public class CateExcelCommandHandler : IRequestHandler<CateExcelCommand<CateExcelDto>, ReturnResult>
{
    private readonly ICatetoryRepository _cateInfoRepository;
    private readonly UserHelper _userHelper;

    public CateExcelCommandHandler(ICatetoryRepository 
        catetoryRepository,
        UserHelper userHelper)
    {
        _cateInfoRepository = catetoryRepository;
        _userHelper = userHelper;
    }

    public async Task<ReturnResult> Handle(CateExcelCommand<CateExcelDto> request, CancellationToken cancellationToken)
    {
        if (!request.Cates.Any())
            throw new Exception("导入的数据无效，请检查导入文件");
        var allUnits= _cateInfoRepository.GetEntities
            .ToList();
        List<CateInfo> units = new List<CateInfo>();
        request.Cates.ForEach(item =>
        {
            var model = allUnits.FirstOrDefault(x => x.CateName.Equals(item.CateName));
            if (model == null && !string.IsNullOrWhiteSpace(item.CateName))
            {
                units.Add(new CateInfo
                {
                    Enable = true,
                    CateName = item.CateName,
                    Remark = item.Remark,
                    CreateUser = _userHelper.LoginName
                });
            }
        });
        await _cateInfoRepository.AddRangeAsync(units);
        int result = await _cateInfoRepository.UnitOfWork.SaveChangesAsync();
        return result >= 0 ? new ReturnResult(true, null, "成功导入【" + units.Count + "】条数据")
            : new ReturnResult(false, null, "导入失败");
    }
}