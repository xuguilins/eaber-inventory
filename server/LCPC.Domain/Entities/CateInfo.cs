
namespace LCPC.Domain.Entities;

public class CateInfo : EntityBase
{
    /// <summary>
    /// 分类名称
    /// </summary>
    public string CateName { get; set; }
    /// <summary>
    /// 上级分类
    // /// </summary>
    public string ParentId { get; set; }
    public virtual ICollection<ProductInfo> ProductInfos { get; set; }
}