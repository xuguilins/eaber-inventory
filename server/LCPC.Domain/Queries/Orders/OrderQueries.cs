using System.Collections;
using System.Data;
using System.Text;
using Dapper;
using Microsoft.Extensions.Primitives;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace LCPC.Domain.Queries;

public class OrderQueries : IOrderQueries
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISqlDapper _sqlDapper;
    private readonly UserHelper _userHelper;
    private readonly ICustomerInfoRepository _customerInfoRepository;

    public OrderQueries(IOrderRepository orderRepository, ISqlDapper sqlDapper, UserHelper userHelper,
        ICustomerInfoRepository customerInfoRepository)
    {
        _orderRepository = orderRepository;
        _sqlDapper = sqlDapper;
        _userHelper = userHelper;
        _customerInfoRepository = customerInfoRepository;
    }

    public async Task<ReturnResult<List<OrderInfoDto>>> GetOrderPage(OrderSearch search, int type = -1)
    {
        Hashtable hs = new Hashtable();
        StringBuilder sb = new StringBuilder();
        hs.Add("@CreateUser", _userHelper.LoginName);

        #region 构建查询条件

        if (type > -1)
        {
            hs.Add("@OrderStatus", type);
            sb.Append(" and OrderStatus=@OrderStatus");
        }

        if (!string.IsNullOrWhiteSpace(search.Tels))
        {
            sb.Append(" and OrderTel like @OrderTel");
            hs.Add("@OrderTel", "%" + search.Tels + "%");
        }

        if (!string.IsNullOrWhiteSpace(search.UserName))
        {
            sb.Append(" and OrderUser like @OrderUser");
            hs.Add("@OrderUser", "%" + search.UserName + "%");
        }

        if (!string.IsNullOrWhiteSpace(search.Price))
        {
            sb.Append(" and OrderMoney=@OrderMoney");
            hs.Add("@OrderMoney", search.Price);
        }

        if (!string.IsNullOrWhiteSpace(search.StartTime) && !string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append(
                " and  (convert(datetime2,OrderTime)>=convert(datetime2,@startTime)  and convert(datetime2,OrderTime)<= convert(datetime2,@endTime))");
            hs.Add("@startTime", search.StartTime);
            hs.Add("@endTime", search.EndTime);
        }

        if (!string.IsNullOrWhiteSpace(search.StartTime) && string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append(
                " and  convert(datetime2,OrderTime)=convert(datetime2,@startTime)");
            hs.Add("@startTime", search.StartTime);
        }

        if (string.IsNullOrWhiteSpace(search.StartTime) && !string.IsNullOrWhiteSpace(search.EndTime))
        {
            sb.Append(
                " and  convert(datetime2,OrderTime)<= convert(datetime2,@endTime)");
            hs.Add("@endTime", search.EndTime);
        }

        #endregion

        string countSql = "select count(Id) as COUNTNUM from OrderInfo where 1=1  and  CreateUser=@CreateUser " +
                          sb.ToString();
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        string sql =
            @"select Id,OrderCode,OrderTime,OrderUser,OrderTel,OrderMoney as OrderPrice, Remark,OrderPay as PayName,OrderClient as PayClient, OrderStatus as Status " +
            "from OrderInfo where 1=1  and CreateUser=@CreateUser" + sb.ToString() +
            " order by convert(datetime2,OrderTime ) desc   OFFSET @start  rows     fetch next @end  rows only";

        hs.Add("@start", (search.PageIndex - 1) * search.PageSize);
        hs.Add("@end", search.PageSize);
        var data = await _sqlDapper.QueryAsync<OrderInfoDto>(sql, hs);
        var result = new ReturnResult<List<OrderInfoDto>>(true, data, "订单获取成功");
        result.TotalCount = count;
        return result;
    }

    public async Task<ReturnResult<List<OrderBuyUsers>>> GetOrderBuys()
    {
        var data = _customerInfoRepository.GetEntities
            .Where(d => d.CreateUser.Equals(_userHelper.LoginName) && d.Enable)
            .OrderByDescending(d => d.CreateTime)
            .Select(d => new OrderBuyUsers
            {
                Id = d.Id,
                Name = d.CustomerName,
                Tel = d.TelNumber ?? d.PhoneNumber
            }).Distinct().ToList();
        return new ReturnResult<List<OrderBuyUsers>>(true, data, "");
    }

    public async Task<ReturnResult<long>> GetOrderCount(OrderStatus status)
    {
        var count = await _orderRepository.GetEntityCountAsync
            (x => x.OrderStatus == status && x.CreateTime.Equals(_userHelper.LoginName));
        return new ReturnResult<long>(true, count, "获取订单数量成功");
    }

    public async Task<ReturnResult<SignleOrderInfo>> GetOrderInfo(string id)
    {
        var order = await _orderRepository.GetSignleOrder(id);
        SignleOrderInfo info = new SignleOrderInfo
        {
            Status = order.OrderStatus,
            Id = order.Id,
            OrderCode = order.OrderCode,
            OrderPrice = order.OrderMoney,
            OrderTel = order.OrderTel,
            OrderTime = order.OrderTime,
            OrderUser = order.OrderUser,
            PayClient = order.OrderClient,
            PayName = order.OrderPay,
            Remark = order.Remark,
            DetailDtos = order.OrderInfoDetails.Select((v, index) => new OrderDetailDto()
            {
                AllPrice = v.OrderPrice,
                Count = v.OrderCount,
                Price = v.OrderSigle,
                ProductCode = v.ProductCode,
                ProductName = v.ProductName,
                UnitName = v.UnitName,
                Remark = v.Remark,
                Index = (index + 1)
            }).ToList()
        };
        return new ReturnResult<SignleOrderInfo>(true, info, "获取指定的订单数据成功");
    }

    public async Task<ReturnResult<List<CustomerOrderDto>>> GetOrderCusPage(CusSearh search)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable hs = new Hashtable();
        string user = _userHelper.LoginName ?? "黄剑";
        hs.Add("@CreateUser", user);
        if (!string.IsNullOrWhiteSpace(search.KeyWord))
        {
            sb.Append(" and  a.CustomerName like @CustomerName ");
            hs.Add("@CustomerName", "%" + search.KeyWord + "%");
        }

        if (search.Status != OrderStatus.ALL)
        {
            sb.Append(" and b.OrderStatus=@status ");
            hs.Add("@status", search.Status);
        }

        string countSql = @$"select count(1) from (select *
                from (select a.CustomerName as CustomerName,
                             a.CustomerUser as CustomerUser,
                             a.Id as UserId,
                             SUM(b.ActuailMoney)
                                            as CustomerMoney,
                             (
                                 case b.OrderStatus
                                     when 0 then '待支付'
                                     when 1 then '已支付'
                                     when 2 then '已完成'
                                     when 3 then '已作废'
                                     else '已取消' end
                                 )          as OrderStatus
                      from CustomerInfo a
                               left join OrderInfo b
                                         on a.Id = b.OrderUserId
                      where 1=1  and b.CreateUser=@CreateUser {sb.ToString()}
                      group by a.CustomerName, a.CustomerUser, b.OrderStatus,a.Id) as s
                         pivot (
                         max(CustomerMoney)
                         for OrderStatus in (待支付,已支付,已完成,已作废,已取消)) as ss) as t";
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);

        int start = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;

        hs.Add("@start", start);
        hs.Add("@end", end);
        string filterSql = @$"select    
          CustomerName,CustomerUser,待支付 as dzf, 已支付 as 
              yzf,已作废 as zf,已取消 as yqx, 
                  已完成 as ywc,UserId from (select *
                from (select a.CustomerName as CustomerName,
                             a.CustomerUser as CustomerUser,
                             
                              a.Id as UserId,
                             SUM(b.ActuailMoney)
                                            as CustomerMoney,
                             (
                                 case b.OrderStatus
                                     when 0 then '待支付'
                                     when 1 then '已支付'
                                     when 2 then '已完成'
                                     when 3 then '已作废'
                                     else '已取消' end
                                 )          as OrderStatus
                      from CustomerInfo a
                               left join OrderInfo b
                                         on a.Id = b.OrderUserId
                      where 1=1  and b.CreateUser=@CreateUser {sb.ToString()}
                      group by a.CustomerName, a.CustomerUser, b.OrderStatus,a.Id) as s
                         pivot (
                         max(CustomerMoney)
                         for OrderStatus in (待支付,已支付,已完成,已作废,已取消)) as ss) as t
               order by t.CustomerName offset @start rows fetch  next @end  rows only;";

        var data =
            await _sqlDapper.QueryAsync<CustomerOrderDto>(filterSql, hs);
        var result = new ReturnResult<List<CustomerOrderDto>>(true, data, "获取与客户往来交易成功");
        result.TotalCount = count;
        return result;
    }

    public async Task<MemoryStream> ExportOrderCus()
    {
        Hashtable hs = new Hashtable();
        string user = _userHelper.LoginName;
        if (string.IsNullOrWhiteSpace(user))
            user = "黄剑";
        hs.Add("@CreateUser", user);
        string filterSql = @$"select    
          CustomerName,CustomerUser,待支付 as dzf, 已支付 as 
              yzf,已作废 as zf,已取消 as yqx, 
                  已完成 as ywc from (select *
                from (select a.CustomerName as CustomerName,
                             a.CustomerUser as CustomerUser,
                             SUM(b.ActuailMoney)
                                            as CustomerMoney,
                             (
                                 case b.OrderStatus
                                     when 0 then '待支付'
                                     when 1 then '已支付'
                                     when 2 then '已完成'
                                     when 3 then '已作废'
                                     else '已取消' end
                                 )          as OrderStatus
                      from CustomerInfo a
                               left join OrderInfo b
                                         on a.Id = b.OrderUserId
                      where 1=1  and b.CreateUser=@CreateUser 
                      group by a.CustomerName, a.CustomerUser, b.OrderStatus) as s
                         pivot (
                         max(CustomerMoney)
                         for OrderStatus in (待支付,已支付,已完成,已作废,已取消)) as ss) as t
               ";
        var data = await _sqlDapper.QueryAsync<CustomerOrderDto>(filterSql, hs);
        MemoryStream ms = new MemoryStream();
        //{
        ms.SaveAs(data);
        ms.Seek(0, SeekOrigin.Begin);
        return ms;
        // }
    }

    public async Task<byte[]> ExportHeightCus(ExportSearch search)
    {
        Hashtable hs = new Hashtable();
        string user = _userHelper.LoginName;
        if (string.IsNullOrWhiteSpace(user))
            user = "黄剑";
        hs.Add("@CreateUser", user);
        StringBuilder sb = new StringBuilder();
        if (search.OrderType != OrderStatus.ALL)
        {
            sb.Append(" and b.OrderStatus=@status ");
            hs.Add("@status", search.OrderType);
        }

        if (!string.IsNullOrWhiteSpace(search.UserName))
        {
            sb.Append(" and a.CustomerName=@Name");
            hs.Add("@Name", search.UserName);
        }

        if (search.RangeTime.Any())
        {
            var start = Convert.ToDateTime(search.RangeTime[0] + " 00:00:00");
            var end = Convert.ToDateTime(search.RangeTime[1] + " 23:59:59");
            sb.Append(" and convert(datetime2,b.OrderTime)>=@start and convert(datetime2,b.OrderTime)<=@end ");
            hs.Add("@start", start);
            hs.Add("@end", end);
        }

        string filterSql =
            @$"select a.CustomerCode, a.CustomerName,a.CustomerUser, a.TelNumber,
       
       (
           case when  b.OrderStatus = 0 then '待支付'
                when  b.OrderStatus = 1  then '已支付' 
             when  b.OrderStatus = 2 then '已完成' 
             when  b.OrderStatus = 3 then '已作废' 
           else  '已取消' end 
           
       ) OrderStatus ,  b.OrderCode, b.OrderTime,  b.OrderUser, b.OrderTel,
       b.OrderPay,b.OrderMoney, c.ProductCode,c.ProductName,c.UnitName,c.OrderCount,c.OrderSigle,c.OrderPrice   from CustomerInfo a left join  OrderInfo b
on a.Id = b.OrderUserId left  join OrderInfoDetail c
on b.Id = c.OrderId  where b.CreateUser=@CreateUser {sb.ToString()}
            order by  a.CustomerCode,b.OrderStatus,b.OrderCode      ";
        var data = await _sqlDapper.QueryAsync<CustomerOrderHeightDto>(filterSql, hs);

        XSSFWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("客户往来详细信息");
        ICellStyle styles = workbook.CreateCellStyle();

        #region 头部

        IRow row = sheet.CreateRow(0);
        ICell cell = row.CreateCell(0);
        cell.SetCellValue("客户编码");
        row.CreateCell(1).SetCellValue("客户名称");
        row.CreateCell(2).SetCellValue("客户联系人");
        row.CreateCell(3).SetCellValue("客户联系方式");
        row.CreateCell(4).SetCellValue("订单状态");
        row.CreateCell(5).SetCellValue("订单编码");
        row.CreateCell(6).SetCellValue("单据时间");
        row.CreateCell(7).SetCellValue("购买单位");
        row.CreateCell(8).SetCellValue("联系方式");
        row.CreateCell(9).SetCellValue("支付方式");
        row.CreateCell(10).SetCellValue("订单金额");
        row.CreateCell(11).SetCellValue("产品编码");
        row.CreateCell(12).SetCellValue("产品名称");
        row.CreateCell(13).SetCellValue("产品单位");
        row.CreateCell(14).SetCellValue("数量");
        row.CreateCell(15).SetCellValue("单价");
        row.CreateCell(16).SetCellValue("总价");

        #endregion

        Dictionary<string, HightDic> dics = new Dictionary<string, HightDic>();
        Dictionary<string, HightDic> codes = new Dictionary<string, HightDic>();
        Dictionary<string, HightDic> orders = new Dictionary<string, HightDic>();

        #region 填充行数据/合并值计算

        for (int i = 0; i < data.Count; i++)
        {
            HightDic info = new HightDic();
            HightDic code = new HightDic();
            HightDic order = new HightDic();
            int j = i + 1;
            var item = data[i];
            IRow nRow = sheet.CreateRow(j);
            nRow.CreateCell(0).SetCellValue(item.CustomerCode);
            nRow.CreateCell(1).SetCellValue(item.CustomerName);
            nRow.CreateCell(2).SetCellValue(item.CustomerUser);
            nRow.CreateCell(3).SetCellValue(item.TelNumber);
            nRow.CreateCell(4).SetCellValue(item.OrderStatus);
            nRow.CreateCell(5).SetCellValue(item.OrderCode);
            nRow.CreateCell(6).SetCellValue(item.OrderTime);
            nRow.CreateCell(7).SetCellValue(item.OrderUser);
            nRow.CreateCell(8).SetCellValue(item.OrderTel);
            nRow.CreateCell(9).SetCellValue(item.OrderPay);
            nRow.CreateCell(10).SetCellValue(item.OrderMoney.ToString());
            nRow.CreateCell(11).SetCellValue(item.ProductCode);
            nRow.CreateCell(12).SetCellValue(item.ProductName);
            nRow.CreateCell(13).SetCellValue(item.UnitName);
            nRow.CreateCell(14).SetCellValue(item.OrderCount);
            nRow.CreateCell(15).SetCellValue(item.OrderSigle.ToString());
            nRow.CreateCell(16).SetCellValue(item.OrderPrice.ToString());
            if (search.MaginType == 1)
            {
                if (!dics.ContainsKey(item.CustomerCode))
                {
                    info.Start = j;
                    info.End = j;
                    dics.Add(item.CustomerCode, info);
                }
                else
                {
                    var value = dics[item.CustomerCode];
                    value.End += 1;
                    dics[item.CustomerCode] = value;
                }

                string key = $"{item.CustomerCode}-{item.OrderStatus}";
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

                key = $"{item.CustomerCode}-{item.OrderCode}";

                if (!orders.ContainsKey(key))
                {
                    order.Start = j;
                    order.End = j;
                    orders.Add(key, order);
                }
                else
                {
                    var value = orders[key];
                    value.End += 1;
                    orders[key] = value;
                }
            }
        }

        #endregion
        
        #region 合并

        if (search.MaginType == 1)
        {
            foreach (var item in dics.Keys)
            {
                var key = item;
                var model = dics[key];
                if (model.End > model.Start)
                {
                    CellRangeAddress arAddress = new CellRangeAddress(model.Start, model.End, 0, 0);
                    CellRangeAddress khMc = new CellRangeAddress(model.Start, model.End, 1, 1);
                    CellRangeAddress khLXR = new CellRangeAddress(model.Start, model.End, 2, 2);
                    CellRangeAddress khlxfs = new CellRangeAddress(model.Start, model.End, 3, 3);
                    sheet.AddMergedRegion(arAddress);
                    sheet.AddMergedRegion(khMc);
                    sheet.AddMergedRegion(khLXR);
                    sheet.AddMergedRegion(khlxfs);
                }
            }

            foreach (var item in codes.Keys)
            {
                var key = item;
                var model = codes[key];
                if (model.End > model.Start)
                {
                    CellRangeAddress khlxfs = new CellRangeAddress(model.Start, model.End, 4, 4);
                    sheet.AddMergedRegion(khlxfs);
                }
            }

            foreach (var item in orders.Keys)
            {
                var key = item;
                var model = orders[key];
                if (model.End > model.Start)
                {
                    CellRangeAddress khlxfs = new CellRangeAddress(model.Start, model.End, 5, 5);
                    sheet.AddMergedRegion(khlxfs);
                }
            }
        }


    

        #endregion
        
        using (MemoryStream ms = new MemoryStream())
        {
            workbook.Write(ms);
            return ms.ToArray();
        }
    
      

    }

    public async Task<ReturnResult<List<OrderInfoDto>>> GetSignOrderUserId(UserOrderSearch search)
    {
        var user = _userHelper.LoginName;
        if (string.IsNullOrWhiteSpace(user))
            user = "黄剑";
        string sql =
            @"select a.Id,a.OrderCode,a.OrderUser,a.OrderUserId, a.OrderTime,a.OrderTel,a.ActuailMoney as OrderPrice,a.Remark,a.OrderPay as PayName,a.OrderClient as PayClient from OrderInfo a  
         where a.CreateUser=@CreateUser and a.OrderUserId=@OrderUserId and a.OrderStatus=@OrderStatus 
         order by  convert(datetime2,a.OrderTime) desc
        offset @start rows fetch next @end rows only";
        int start = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", user);
        hs.Add("@OrderStatus", search.Status);
        hs.Add("@OrderUserId", search.UserId);
        string countSql = @"select count(a.Id) from OrderInfo a  
         where a.CreateUser=@CreateUser and a.OrderUserId=@OrderUserId and a.OrderStatus=@OrderStatus";
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        hs.Add("@start", start);
        hs.Add("@end", end);
        var data = await _sqlDapper.QueryAsync<OrderInfoDto>(sql, hs);

        var result = new ReturnResult<List<OrderInfoDto>>(true, data, "获取数据成功");
        result.TotalCount = count;
        return result;
    }

    public async Task<ReturnResult<List<OrderDetailDto>>> GetSignOrderUserDetail(string orderId)
    {
        string sql =
            @"    select  b.ProductName,b.ProductCode,b.OrderCount as Count,b.UnitName,b.OrderSigle as Price,b.Remark,b.OrderPrice as AllPrice,b.OrderId as ParentId from OrderInfoDetail b
where b.OrderId =@OrderId";
        var data = await _sqlDapper.QueryAsync<OrderDetailDto>(sql, new { OrderId = orderId });
        return new ReturnResult<List<OrderDetailDto>>(true, data, "获取指定的订单明细成功");
    }
}