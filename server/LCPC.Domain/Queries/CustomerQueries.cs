namespace LCPC.Domain.Queries;

public class CustomerQueries:ICustomerQueries
{
    private readonly ICustomerInfoRepository _customerInfoRepository;
    public CustomerQueries(ICustomerInfoRepository cyCustomerInfoRepository)
    {
        _customerInfoRepository = cyCustomerInfoRepository;
    }
    public async Task<ReturnResult<List<CustomerDto>>> GetCustomerPages(DataSearch search)
    {
        long count = await _customerInfoRepository.GetEntities
            .CountAsyncIf(!string.IsNullOrWhiteSpace(search.KeyWord),
                d => d.CustomerName.Contains(search.KeyWord));
        int skip = (search.PageIndex - 1) * search.PageSize;
        int take = search.PageSize;
        var list = _customerInfoRepository.GetEntities
            .WhereIf(!string.IsNullOrWhiteSpace(search.KeyWord),
                d => d.CustomerName.Contains(search.KeyWord,
                         StringComparison.CurrentCultureIgnoreCase) ||
                     d.NameSpell.Contains(search.KeyWord,StringComparison.CurrentCultureIgnoreCase))
            .OrderByDescending(d=>d.CreateTime)
            .Skip(skip).Take(take)
            .Select(m => new CustomerDto
            {
                Id = m.Id,
                CuTel = m.TelNumber,
                CuAddress = m.Address,
                CuCode = m.CustomerCode,
                CuName = m.CustomerName,
                Remark = m.Remark,
                Enable = m.Enable,
                CuPhone = m.PhoneNumber,
                CuUser = m.CustomerUser
            }).ToList();

        var result =new ReturnResult<List<CustomerDto>>(true, list, "分页获取客户信息成功");
        result.TotalCount = count;
        return result;

    }
}
