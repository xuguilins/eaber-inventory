
using System.IO;
using System.Linq;
using LCPC.Domain.Commands.Providers;
using LCPC.Domain.QueriesDtos;
using MiniExcelLibs;

namespace LCPC.Domain.Commands
{
    public interface IExcelCommand
    {

    }
    public class CommandBuilder
    {
        public static IExcelCommand CreaeCommand(int type, byte[] bytes)
        {

            IExcelCommand command = null;
            switch (type)
            {
                case 1: //供应商
                    command =buildSupilers(bytes);
                    break; 
                case 2: //单位
                    //command =buildUnits(bytes);
                    break;
                case 3: //分类
                    command = buildCate(bytes);
                    break;
                case 5: // 产品
                    command = buildProduct(bytes);
                    break;
                
            }

            return command;
        }
        #region 供应商导入
        private static ProviderExcelCommand<SupilerExcelDto> buildSupilers(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
          
                ms.Write(bytes);
                var rows = ms.Query<SupilerExcelDto>()
                .ToList();
                ProviderExcelCommand<SupilerExcelDto> cmd = new ProviderExcelCommand<SupilerExcelDto>
                {
                    Providers = rows
                };
                return cmd;
            }
        }
        #endregion
        
        #region 单位导入
        // private static UnitExcelCommand<UnitExcelDto> buildUnits(byte[] bytes)
        // {
        //     using (MemoryStream ms = new MemoryStream())
        //     {
        //         ms.Write(bytes);
        //         var rows = ms.Query<UnitExcelDto>(null, excelType: ExcelType.XLSX)
        //             .ToList();
        //         UnitExcelCommand<UnitExcelDto> cmd = new UnitExcelCommand<UnitExcelDto>
        //         {
        //             Units = rows
        //         };
        //         return cmd;
        //     }
        // }
        #endregion

        #region 分类导入

        private static CateExcelCommand<CateExcelDto> buildCate(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes);
                var rows = ms.Query<CateExcelDto>(null, excelType: ExcelType.XLSX)
                    .ToList();
                CateExcelCommand<CateExcelDto> cmd = new CateExcelCommand<CateExcelDto>
                {
                    Cates = rows
                };
                return cmd;
            }
        }

        #endregion
        
        #region 产品导入

        private static ProductExcelCommand<ProductExcelDto> buildProduct(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes);
                var rows = ms.Query<ProductExcelDto>(null, excelType: ExcelType.XLSX)
                    .ToList();
                ProductExcelCommand<ProductExcelDto> cmd = new ProductExcelCommand<ProductExcelDto>
                {
                    Products = rows
                };
                return cmd;
            }
        }

        #endregion
    }
}