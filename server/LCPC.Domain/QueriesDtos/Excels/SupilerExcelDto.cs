using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniExcelLibs.Attributes;

namespace LCPC.Domain.QueriesDtos
{
    public class SupilerExcelDto
    {

        [ExcelColumnName("供应商名称")]
        public string SupileName { get; set; }
        [ExcelColumnName("手机号1")]
        public string TelONE { get; set; }
         [ExcelColumnName("手机号2")]
        public string TelTWO { get; set; }
        [ExcelColumnName("座机号1")]
        public string PhoneONE { get; set; }
        [ExcelColumnName("座机号2")]
        public string PhoneTWO { get; set; }
        [ExcelColumnName("供应商联系人1")]
        public string UserONE { get; set; }
        [ExcelColumnName("供应商联系人2")]
        public string UserTWO { get; set; }
        [ExcelColumnName("联系地址")]
        public string Address { get; set; }
        [ExcelColumnName("备注")]
        public string Remark { get; set; }
    }
}