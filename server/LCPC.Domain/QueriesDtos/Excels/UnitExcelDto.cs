using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniExcelLibs.Attributes;

namespace LCPC.Domain.QueriesDtos
{
    public class UnitExcelDto
    {
        [ExcelColumnName("单位名称")]
        public string UnitName { get; set; }
        [ExcelColumnName("描述")]

        public string Remark { get; set; }
    }
}