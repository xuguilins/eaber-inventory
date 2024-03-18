using MiniExcelLibs.Attributes;

namespace LCPC.Domain.QueriesDtos;

public class CateExcelDto
{
    [ExcelColumnName("分类名称")]
    public string CateName { get; set; }
    [ExcelColumnName("描述")]

    public string Remark { get; set; }
}