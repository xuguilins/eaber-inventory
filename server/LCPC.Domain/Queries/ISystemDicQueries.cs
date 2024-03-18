namespace LCPC.Domain.Queries;

public class DicSearch:DataSearch
{
    public DicType DicType { get; set; }
    
}
public interface ISystemDicQueries:IScopeDependecy
{
    Task<ReturnResult<List<SystemDicDto>>> GetSystemDicPage(DicSearch search);

    Task<ReturnResult<List<DicModel>>> GetDicTypes();

    Task<ReturnResult<List<DicModel>>> GetRuleTypes();
    Task<ReturnResult<List<UnitInfoDto>>> GetUnits();

    Task<ReturnResult<List<SysDicData>>> GetDics(DicType type);
 
    Task<ReturnResult<List<EnablePayInfoDto>>> GetPays();
    Task test();
}