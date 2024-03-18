using System.Collections;
using System.Text;
using Microsoft.Extensions.Primitives;
using MiniExcelLibs;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace LCPC.Domain.Queries;

public class PuraseInOrderQueries : IPuraseInOrderQueries
{
    private readonly IPurchaseInRepository _purchaseInRepository;
    private readonly ISqlDapper _sqlDapper;
    private readonly UserHelper _userHelper;
    private readonly IPurchaseInDetailRepository _purchaseInDetailRepository;

    public PuraseInOrderQueries(IPurchaseInRepository purchaseInRepository, ISqlDapper sqlDapper, UserHelper userHelper,
        IPurchaseInDetailRepository detailRepository)
    {
        _purchaseInRepository = purchaseInRepository;
        _sqlDapper = sqlDapper;
        _userHelper = userHelper;
        _purchaseInDetailRepository = detailRepository;
    }

    public async Task<ReturnResult<List<PuraseInOrderDto>>> GetInPurasePage(PuraseSearch search)
    {
        int start = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;
        string filterSql =
            @"select a.PurchaseCode as InOrderCode,a.InPhone,a.InOStatus,a.InUser,a.Logistics,
        a.InCount  as AllCount
       ,a.InPrice as AllPrice ,a.Id,
        a.InOrderTime as InTime,a.ChannelType as Chanpel,a.Remark,b.SupName as SupileName,a.SupplierId
        from  PurchaseInOrder a join SupplierInfo b on a.SupplierId = b.Id where 1=1  and a.CreateUser=@CreateUser  {0} 
     order by a.InOrderTime desc offset  @start   rows fetch  next @end  rows only;";
        string countSql =
            @"select  count(a.Id) as VCount from  PurchaseInOrder 
    a join SupplierInfo b on a.SupplierId = b.Id where 1=1  and a.CreateUser=@CreateUser {0}";
        var builder = buidlFilter(search);

        long totalCount = await _sqlDapper.QueryLongCountAsync(string.Format(countSql, builder.builder), builder.hs);
        builder.hs.Add("@start", start);
        builder.hs.Add("@end", end);
        var list = await _sqlDapper.QueryAsync<PuraseInOrderDto>(string.Format(filterSql, builder.builder), builder.hs);
        ReturnResult<List<PuraseInOrderDto>> result = new ReturnResult<List<PuraseInOrderDto>>(true, list)
        {
            TotalCount = totalCount,
            Message = "获取进货单成功"
        };
        return
            result;
    }

    public async Task<ReturnResult<PuraseOutDto>> GetPuraseInfo(string id)
    {
        string sql = @"select  a.PurchaseCode as InOrderCode , a.InOrderTime as InOrderTime,
        a.SupplierId as SupplierId , a.InPhone as InPhone,  a.InCount as InCount,
        a.Logistics as Logistics, a.InPrice as InPrice,
        a.InUser as InUser,  a.InOStatus as InOStatus,  a.Remark as Remark, a.ChannelType as ChannelType, a.Id,  b.SupName as SupileName
        from PurchaseInOrder  a join SupplierInfo b
        on a.SupplierId = b.Id where a.Id=@Id";
        var dto = await _sqlDapper.QueryFirstAsync<PuraseOutDto>(sql, new { Id = id });
        if (dto == null)
            throw new Exception("未找到有效的数据");
        dto.PrdocutDetail = new List<PurashDetailOutDto>();
        string detialSql =
            @"select b.Id,c.ProductCode as ProductCode,c.ProductModel as ProductModel, c.ProductName as ProductName,
        b.ProductCount as ProductCount,d.Id as UnitId,
       d.DicCode as  UnitName,e.Id as CateId,e.CateName,c.InitialCost as ProductIncost,
        c.Wholesale as ProductWocost,b.ProductPrice as ProductPrice,b.ProductAll as ProductAll,
        c.SellPrice as SellPrice ,c.InventoryCount as InvertCount   from  PurchaseInDetail b  join ProductInfo c
on b.ProductId = c.Id and b.ProductCode =c.ProductCode
join (select  Id,DicCode,DicName   from SystemDicInfo  where Enable=1  and DicType=1) d on c.UnitId = d.Id join  CateInfo e
on e.Id = c.CateId where b.PurchaseInId=@PurchaseInId";
        dto.PrdocutDetail = await _sqlDapper.QueryAsync<PurashDetailOutDto>(detialSql, new { PurchaseInId = id });
        return new ReturnResult<PuraseOutDto>(true, dto, "获取指定的进货单成功");
    }

    public async Task<ReturnResult<List<PuraseOutOrderDto>>> GetOutPurasePage(PuraseSearch search)
    {
        var filterData = buildOutFilter(search);
        Hashtable hs = filterData.hs;
        string whereStr = filterData.builder;
        string countSql = @"select count(a.Id)
        from PurchaseOutOrder a join SupplierInfo b
        on a.SupilerId = b.Id where a.CreateUser=@CreateUser " + whereStr;
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        int start = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;
        hs.Add("@start", start);
        hs.Add("@end", end);
        string filterSql = @$"select a.Id,a.PurchaseCode as OutOrderCode,a.OrderTime as OutTime,
        a.Logicse as Logistics, a.InUser as OutUser,a.InPhone as OutPhone,
        b.SupName as SupileName,a.SupilerId as SupplierId,a.OutOrderCount as OutAllCount,
        a.OutOrderPrice as OutAllPrice,a.OutStatus as OutStatus,
        a.Remark
        from PurchaseOutOrder a join SupplierInfo b
        on a.SupilerId = b.Id where a.CreateUser=@CreateUser  {whereStr}
                              order by a.OrderTime  desc offset  @start   rows fetch  next @end  rows only";
        var data = await _sqlDapper.QueryAsync<PuraseOutOrderDto>(filterSql, hs);
        var result = new ReturnResult<List<PuraseOutOrderDto>>(true, data, "分页获取退货单成功");
        result.TotalCount = count;
        return result;
    }

    public async Task<ReturnResult<List<PurasheOutModalDto>>> GetPurashModalPage(DataSearch search)
    {
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", _userHelper.LoginName);
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(search.KeyWord))
        {
            sb.Append(" and InUser like @UserName or InPhone like @UserTel or PurchaseCode like @PurchaseCode");
            hs.Add("UserName", "%" + search.KeyWord + "%");
            hs.Add("UserTel", "%" + search.KeyWord + "%");
            hs.Add("PurchaseCode", "%" + search.KeyWord + "%");
        }

        string countSql = @"select count(Id)  from  PurchaseInOrder
where  CreateUser=@CreateUser and PurchaseCode not in (select  InOrderCode from PurchaseOutOrder)
      " + sb.ToString();
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        int start = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;
        string filterSql =
            "select Id,InUser as UserName,InPhone as UserTel,InOrderTime as PushTime,PurchaseCode as code,SupplierId  as supilerId from  PurchaseInOrder " +
            "where CreateUser=@CreateUser and  PurchaseCode not in (select  InOrderCode from PurchaseOutOrder)   " +
            sb.ToString() + "" +
            " order by Id desc OFFSET @start ROWS FETCH NEXT @end ROWS ONLY";
        hs.Add("@start", start);
        hs.Add("@end", end);
        var data = await _sqlDapper.QueryAsync<PurasheOutModalDto>(filterSql, hs);
        var result = new ReturnResult<List<PurasheOutModalDto>>(true, data, "获取数据成功");
        result.TotalCount = count;
        return result;
    }

    private (Hashtable hs, string builder) buidlFilter(PuraseSearch search)
    {
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", _userHelper.LoginName);
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(search.SupileName))
        {
            sb.Append(" and  b.SupName like @SupName");
            hs.Add("@SupName", $"%{search.SupileName}%");
        }

        if (!string.IsNullOrWhiteSpace(search.Tel))
        {
            sb.Append(" and a.InPhone like @InPhone ");
            hs.Add("@InPhone", $"%{search.Tel}%");
        }

        if (!string.IsNullOrWhiteSpace(search.UserName))
        {
            sb.Append(" and a.InUser like @InUser ");
            hs.Add("@InUser", $"%{search.UserName}%");
        }

        if (!string.IsNullOrWhiteSpace(search.StartTime) && string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append(" and convert(datetime2,InOrderTime)>=convert(datetime2,@InOrderTime) ");
            hs.Add("@InOrderTime", search.StartTime);
        }

        if (!string.IsNullOrWhiteSpace(search.EndTime) && string.IsNullOrWhiteSpace(search.StartTime))
        {
            sb.Append(" and convert(datetime2,InOrderTime)<=convert(datetime2,@InOrderTime) ");
            hs.Add("@InOrderTime", search.EndTime);
        }

        if (!string.IsNullOrWhiteSpace(search.StartTime) && !string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append(@" and (convert(datetime2,InOrderTime)>=convert(datetime2,@StartTime) and
       convert(datetime2,InOrderTime)<=convert(datetime2,@EndTime)) ");
            hs.Add("@StartTime", search.StartTime);
            hs.Add("@EndTime", search.EndTime);
        }

        return (
            hs, sb.ToString());
    }

    private (Hashtable hs, string builder) buildOutFilter(PuraseSearch search)
    {
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", _userHelper.LoginName);
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(search.SupileName))
        {
            sb.Append(" and  b.SupName like @SupName");
            hs.Add("@SupName", $"%{search.SupileName}%");
        }

        if (!string.IsNullOrWhiteSpace(search.Tel))
        {
            sb.Append("and a.InPhone like @InPhone ");
            hs.Add("@InPhone", $"%{search.Tel}%");
        }

        if (!string.IsNullOrWhiteSpace(search.UserName))
        {
            sb.Append(" and a.InUser like @InUser ");
            hs.Add("@InUser", $"%{search.UserName}%");
        }

        if (!string.IsNullOrWhiteSpace(search.StartTime) && string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append("  and convert(datetime2,OrderTime)>=convert(datetime2,@InOrderTime) ");
            hs.Add("@InOrderTime", search.StartTime);
        }

        if (!string.IsNullOrWhiteSpace(search.EndTime) && string.IsNullOrWhiteSpace(search.StartTime))
        {
            sb.Append(" and convert(datetime2,OrderTime)<=convert(datetime2,@InOrderTime) ");
            hs.Add("@InOrderTime", search.EndTime);
        }

        if (!string.IsNullOrWhiteSpace(search.StartTime) && !string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append(@" and (convert(datetime2,OrderTime)>=convert(datetime2,@StartTime) and
       convert(datetime2,OrderTime)<=convert(datetime2,@EndTime)) ");
            hs.Add("@StartTime", search.StartTime);
            hs.Add("@EndTime", search.EndTime);
        }

        return (
            hs, sb.ToString());
    }

    public async Task<ReturnResult<List<PurasheOutModalDetail>>> GetPurashModalDetail(string id)
    {
        var data = await _purchaseInDetailRepository.GetEntitiesAsync(d => d.PurchaseInId.Equals(id));
        var list = data.Select(d => new PurasheOutModalDetail
        {
            InPrice = d.ProductPrice,
            OutPrice = d.ProductPrice,
            ProductCount = d.ProductCount,
            ReturnCount = d.ProductCount,
            ProductModel = d.ProductModel,
            ProductName = d.ProductName,
            OutAll = (d.ProductCount) * d.ProductPrice,
            ProductCode = d.ProductCode,
            Id = d.Id
        }).ToList();
        return new ReturnResult<List<PurasheOutModalDetail>>(true, list, "获取单据商品成功");
    }

    public async Task<ReturnResult<PuraseOutOrderSigleDto>> GetSigleOutPurashInfo(string id)
    {
        string mainSql =
            @"SELECT a.Id,PurchaseCode as OutOrderCode,InOrderCode ,a.Remark as Reason,InPhone,OrderTime as OutOrderTime,
    InUser,Logicse as Logics,a.SupilerId as SupilerId,b.SupName as SupildName,a.OutStatus
       FROM PurchaseOutOrder a  join SupplierInfo b
       on a.SupilerId  =  b.Id  where a.Id=@Id";
        Hashtable hs = new Hashtable();
        hs.Add("@Id", id);
        var data = await _sqlDapper.QueryFirstAsync<PuraseOutOrderSigleDto>(mainSql, hs);
        if (data == null)
            throw new Exception("未找到有效的数据");

        string detailSql = @" 
select Id,InPrice,OutPrice,ProductCode,InCount as ProductCount, OutCount as ReturnCount,ProductModel,ProductName   from PurchaseOutOrderDetail where PurchaseId=@PurchaseId";
        var detail = await _sqlDapper.QueryAsync<PuraseOutOrderSigleDetail>(detailSql, new { PurchaseId = data.Id });
        data.Details = detail;
        return new ReturnResult<PuraseOutOrderSigleDto>(true, data, "获取退货单成功");
    }

    public async Task<ReturnResult<List<PurashInOrderDtoRecord>>> GetSupileCusPage(DataSearch search)
    {
        var user = _userHelper.LoginName;
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", user);
        string whereStr = string.Empty;
        if (!string.IsNullOrWhiteSpace(search.KeyWord))
        {
            hs.Add("@SupName", "%" + search.KeyWord + "%");
            whereStr = " and  a.SupName like  @SupName";
        }

        string countSql = @"select count(1)  from ( select a.SupName, a.Id,
      (   case when  sum(b.InPrice) is null then 0 else  sum(b.InPrice)  end )  ORDERPRICE,
       (   case when  b.InOStatus =1 then  '进行中'  else '已完成' end  ) as OrderStatus from SupplierInfo a left join  
           PurchaseInOrder b on a.Id = b.SupplierId where 1=1 and   a.CreateUser=@CreateUser 
            " + whereStr +
                          " group by a.SupName,a.Id,b.InOStatus) t pivot ( max(t.ORDERPRICE) for t.OrderStatus in (已完成,进行中)) as ss";
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        string filterSql =
            @"select SupName,已完成 as YWC,进行中 JXZ, ProviderUser,SupPhone,SupId from ( select a.SupName, a.Id as SupId,a.ProviderUser,a.SupPhone,
      (   case when  sum(b.InPrice) is null then 0 else  sum(b.InPrice)  end )  ORDERPRICE,
       (   case when  b.InOStatus =1 then  '进行中'  else '已完成' end  ) as OrderStatus from SupplierInfo a left join  
           PurchaseInOrder b on a.Id = b.SupplierId where 1=1 and   a.CreateUser=@CreateUser 
            " + whereStr +
            " group by a.SupName,a.Id,b.InOStatus,a.ProviderUser,a.SupPhone) t pivot ( max(t.ORDERPRICE) for t.OrderStatus in (已完成,进行中)) as ss order by ss.SupName offset @start rows fetch next @end rows only ";

        int start = (search.PageIndex - 1) * search.PageSize;
        hs.Add("@start", start);
        hs.Add("@end", search.PageSize);
        var data = await _sqlDapper.QueryAsync<PurashInOrderDtoRecord>(filterSql, hs);
        var result = new ReturnResult<List<PurashInOrderDtoRecord>>(true, data, "获取供应往来交易成功");
        result.TotalCount = count;
        return result;
    }

    public async Task<byte[]> ExportCusExcel()
    {
        string sql =
            @"select SupName,已完成 as YWC,进行中 JXZ, ProviderUser,SupPhone from ( select a.SupName, a.Id as SupId,a.ProviderUser,a.SupPhone,
      (   case when  sum(b.InPrice) is null then 0 else  sum(b.InPrice)  end )  ORDERPRICE,
       (   case when  b.InOStatus =1 then  '进行中'  else '已完成' end  ) as OrderStatus from SupplierInfo a left join  
           PurchaseInOrder b on a.Id = b.SupplierId where 1=1   and   a.CreateUser=@CreateUser   group by a.SupName,a.Id,b.InOStatus,a.ProviderUser,a.SupPhone) t pivot ( max(t.ORDERPRICE) for
    t.OrderStatus in (已完成,进行中)) as ss order by ss.SupName ";
        var data = await _sqlDapper.QueryAsync<PurashInOrderDtoRecord>(sql,
            new { CreateUser = _userHelper.LoginName });
        using (MemoryStream ms = new MemoryStream())
        {
            ms.SaveAs(data, true, "供应商往来交易(进货)");
            return ms.ToArray();
        }
    }

    public async Task<byte[]> ExportCusHegithExcel(PurashExcelSearch search)
    {
        Hashtable hs = new Hashtable();
        string user = _userHelper.LoginName;
        if (string.IsNullOrWhiteSpace(user))
            user = "黄剑";
        hs.Add("@CreateUser", user);
        StringBuilder sb = new StringBuilder();
        if (search.InorOStatus != InOStatus.ALL)
        {
            sb.Append(" and b.InOStatus = @status ");
            hs.Add("@status", search.InorOStatus);
        }

        if (!string.IsNullOrWhiteSpace(search.UserName))
        {
            sb.Append(" and a.SupName=@SupName ");
            hs.Add("@SupName", search.UserName);
        }

        if (search.RangeTime.Any() && search.RangeTime.Count > 1)
        {
            sb.Append(" and  convert(datetime2,InOrderTime)>=@start and convert(datetime2,InOrderTime)<=@end ");
            var start = Convert.ToDateTime(search.RangeTime[0] + " 00:00:00");
            var end = Convert.ToDateTime(search.RangeTime[1] + " 23:59:59");
            hs.Add("@start", start);
            hs.Add("@end", end);
        }

        string sql =
            @" select a.SupNumber,a.SupName,a.ProviderUser, (case when  b.InOStatus = 1 then '进行中' else '已完成' end ) InOStatus , b.InOrderTime, b.PurchaseCode, b.InCount,  b.InPrice, c.ProductCode,c.ProductName,c.ProductModel,
       c.ProductCount,c.ProductPrice,c.ProductPrice,c.ProductAll
from SupplierInfo a left join PurchaseInOrder b
on a.Id = b.SupplierId  left  join  PurchaseInDetail c
on b.Id = c.PurchaseInId where b.CreateUser=@CreateUser " + sb.ToString()+" order by a.SupNumber";

      
        var data = await _sqlDapper.QueryAsync<PurashInExcelDetail>(sql, hs);

        #region 构建头部

        HSSFWorkbook workbook = new HSSFWorkbook();
        var sheet = workbook.CreateSheet("供应商往来交易(进货)");
        ICellStyle style = workbook.CreateCellStyle();
        IRow header = sheet.CreateRow(0);
        style.FillForegroundColor = HSSFColor.Blue.Index2;
        style.FillPattern = FillPattern.SolidForeground;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.VerticalAlignment = VerticalAlignment.Center;
        IFont font = workbook.CreateFont();
        font.Color = HSSFColor.White.Index;
        style.SetFont(font);
        ICell cell = header.CreateCell(0);
        cell.CellStyle = style;
        cell.SetCellValue("供应商编码");
        cell = header.CreateCell(1);
        cell.CellStyle = style;
        cell.SetCellValue("供应商名称");
        cell = header.CreateCell(2);
        cell.CellStyle = style;
        cell.SetCellValue("联系人");
        cell = header.CreateCell(3);
        cell.CellStyle = style;
        cell.SetCellValue("单据状态");
        cell = header.CreateCell(4);
        cell.CellStyle = style;
        cell.SetCellValue("单据日期");
        cell = header.CreateCell(5);
        cell.CellStyle = style;
        cell.SetCellValue("单据编码");
        cell = header.CreateCell(6);
        cell.CellStyle = style;
        cell.SetCellValue("进货总数");
        cell = header.CreateCell(7);
        cell.CellStyle = style;
        cell.SetCellValue("进货总价");
        cell = header.CreateCell(8);
        cell.CellStyle = style;
        cell.SetCellValue("商品编码");
        cell.CellStyle = style;
        cell = header.CreateCell(9);
        cell.CellStyle = style;
        cell.SetCellValue("商品名称");
        cell = header.CreateCell(10);
        cell.CellStyle = style;
        cell.SetCellValue("商品规格");
        cell = header.CreateCell(11);
        cell.CellStyle = style;
        cell.SetCellValue("本单进货数量");
        cell = header.CreateCell(12);
        cell.CellStyle = style;
        cell.SetCellValue("本单进货金额");
        cell.CellStyle = style;
        cell = header.CreateCell(13);
        cell.CellStyle = style;
        cell.SetCellValue("本单进货总价");

        #endregion

        #region 填充数据

        Dictionary<string, HightDic> users = new Dictionary<string, HightDic>();
        Dictionary<string, HightDic> status = new Dictionary<string, HightDic>();
        Dictionary<string, HightDic> codes = new Dictionary<string, HightDic>();
        for (int i = 0; i < data.Count; i++)
        {
            HightDic info = new HightDic();
            HightDic statu = new HightDic();
            HightDic code = new HightDic();
            int j = i + 1;
            var item = data[i];
            IRow row = sheet.CreateRow(j);
            row.CreateCell(0).SetCellValue(item.SupNumber);
            row.CreateCell(1).SetCellValue(item.SupName);
            row.CreateCell(2).SetCellValue(item.ProviderUser);
            row.CreateCell(3).SetCellValue(item.InOStatus);
            row.CreateCell(4).SetCellValue(item.InOrderTime);
            row.CreateCell(5).SetCellValue(item.PurchaseCode);
            row.CreateCell(6).SetCellValue(item.InCount);
            row.CreateCell(7).SetCellValue(item.InPrice);
            row.CreateCell(8).SetCellValue(item.ProductCode);
            row.CreateCell(9).SetCellValue(item.ProductName);
            row.CreateCell(10).SetCellValue(item.ProductModel);
            row.CreateCell(11).SetCellValue(item.ProductCount);
            row.CreateCell(12).SetCellValue(item.ProductPrice);
            row.CreateCell(13).SetCellValue(item.ProductAll);
            if (search.MaginType == 1)
            {
                string key = item.SupNumber;

                #region 供应商合并

                if (!users.ContainsKey(key))
                {
                    info.Start = j;
                    info.End = j;
                    users.Add(key, info);
                }
                else
                {
                    var value = users[key];
                    value.End += 1;
                    users[key] = value;
                }

                #endregion

                #region 状态合并

                key = $"{item.SupNumber}-{item.InOStatus}";
                if (!status.ContainsKey(key))
                {
                    statu.Start = j;
                    statu.End = j;
                    status.Add(key, statu);
                }
                else
                {
                    var value = status[key];
                    value.End += 1;
                    status[key] = value;
                }

                #endregion

                #region 单据编号合并

                key = $"{item.SupNumber}-{item.PurchaseCode}";
                if (!codes.ContainsKey(key))
                {
                    code.Start = j;
                    code.End = j;
                    codes.Add(key, code);
                }
                else
                {
                    var value = codes[key];
                    value.End += 1;
                    codes[key] = value;
                }

                #endregion
            }
        }

        #endregion

        if (search.MaginType == 1)
        {
            foreach (var key in users.Keys)
            {
                var item = users[key];
                if (item.End > item.Start)
                {
                    CellRangeAddress address = new CellRangeAddress(item.Start, item.End, 0, 0);
                    CellRangeAddress mc = new CellRangeAddress(item.Start, item.End, 1, 1);
                    CellRangeAddress lxr = new CellRangeAddress(item.Start, item.End, 2, 2);
                    sheet.AddMergedRegion(address);
                    sheet.AddMergedRegion(mc);
                    sheet.AddMergedRegion(lxr);
                }
            }

            foreach (var key in status.Keys)
            {
                var item = status[key];
                if (item.End > item.Start)
                {
                    CellRangeAddress address = new CellRangeAddress(item.Start, item.End, 3, 3);
                    sheet.AddMergedRegion(address);
                }
            }

            foreach (var key in codes.Keys)
            {
                var item = codes[key];
                if (item.End > item.Start)
                {
                    CellRangeAddress rq = new CellRangeAddress(item.Start, item.End, 4, 4);
                    CellRangeAddress address = new CellRangeAddress(item.Start, item.End, 5, 5);
                    CellRangeAddress sl = new CellRangeAddress(item.Start, item.End, 6, 6);
                    CellRangeAddress zj = new CellRangeAddress(item.Start, item.End, 7, 7);
                    sheet.AddMergedRegion(address);
                    sheet.AddMergedRegion(rq);
                    sheet.AddMergedRegion(sl);
                    sheet.AddMergedRegion(zj);
                }
            }
        }
        using (MemoryStream ms = new MemoryStream())
        {
            workbook.Write(ms);
            return ms.ToArray();
        }
    }

    public async Task<ReturnResult<List<PurashInOrderTabDetail>>> GetSupileCusTable(SupPuSearch search)
    {
        int start = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;
        Hashtable hs = new Hashtable();
        hs.Add("@SupplierId", search.SupId);
        hs.Add("@CreateUser", _userHelper.LoginName);
        hs.Add("@status", search.Status);

        string countSql = @"  select count(a.Id)  from PurchaseInOrder a join SupplierInfo b
on a.SupplierId =b.Id where a.CreateUser=@CreateUser and a.SupplierId=@SupplierId and a.InOStatus=@status";
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        string sql =
            @"  select  a.Id, PurchaseCode,InOrderTime,ChannelType, b.SupName,a.InUser,a.InPhone,a.InCount,a.InPrice   from PurchaseInOrder a join SupplierInfo b
on a.SupplierId =b.Id where a.CreateUser=@CreateUser and a.SupplierId=@SupplierId  and   a.InOStatus=@status  order by  PurchaseCode desc offset  @start  rows fetch  next @end rows only ;";
        hs.Add("@start", start);
        hs.Add("@end", end);
        var data = await _sqlDapper.QueryAsync<PurashInOrderTabDetail>(sql, hs);
        var result = new ReturnResult<List<PurashInOrderTabDetail>>(true, data, "获取供应商往来交易订单成功");
        result.TotalCount = count;
        return result;
    }

    public async Task<ReturnResult<List<PurashInOrderDetailRecord>>> GetSupileCusDetail(string Id)
    {
        string sql =
            @"select Id,ProductCode,ProductName,ProductModel,ProductCount,ProductPrice,ProductAll  from PurchaseInDetail where PurchaseInId=@PurchaseInId";
        var data = await _sqlDapper.QueryAsync<PurashInOrderDetailRecord>(sql, new { PurchaseInId = Id });
        return new ReturnResult<List<PurashInOrderDetailRecord>>(true, data, "获取订单明细成功");
    }
}