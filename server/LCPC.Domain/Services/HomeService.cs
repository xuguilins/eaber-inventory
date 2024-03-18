using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;
using LCPC.Share;
using LCPC.Share.Configs;
using Microsoft.Extensions.Configuration;
using TinyPinyin;

namespace LCPC.Domain.Services;

public class HomeService : IHomeService
{
    private readonly ISqlDapper _sqlDapper;
    private readonly UserHelper _userHelper;
    private readonly IConfiguration _configuration;
    private static int[] Months = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    public HomeService(ISqlDapper sqlDapper, 
        UserHelper userHelper,
        IConfiguration configuration)
    {
        _sqlDapper = sqlDapper;
        _userHelper = userHelper;
        _configuration = configuration;
    }

    public async Task<ReturnResult<HomeCardDto>> GetHomeStatic()
    {
        UtilHelper.GetWeekRange();
        HomeCardDto dto = new HomeCardDto();
        string user = _userHelper.LoginName;
        //商品总数
        dto.ProductCount = await GetProductStatic(user);
        // 销售总额 
        dto.SellPrice = await GetSellPrices(user);
        //利润
        dto.ProfilePrice = await GetProfilesPrices(user);
        // 订单数量
        dto.OrderCount = await GetOrderStatic(user);
        return new ReturnResult<HomeCardDto>(true, dto, "获取首页卡片统计成功");
    }

    public async Task<ReturnResult<ColumnCardDto>> GetColumns(ColumnsDto dto)
    {
      
        ColumnCardDto info = new ColumnCardDto();
        string user = _userHelper.LoginName;
        switch (dto.ColumnType)
        {
            case ColumnType.SellType:
                info = await GetSellColumns(user, dto.FilterType,dto.StartTime,dto.EndTime);
                break;
            case ColumnType.InProduct:
                info = await GetPurashColumns(user, dto.FilterType, dto.StartTime, dto.EndTime);
                break;
        }

        return new ReturnResult<ColumnCardDto>(true, info, "获取柱状图统计成功");
    }

    #region 销售额柱状图统计

    private async Task<ColumnCardDto> GetSellColumns(string user, FilterType type,string start,string end)
    {
        List<ColumnsQueryDto> colums = new List<ColumnsQueryDto>();
        var months = UtilHelper.GetMonths;
        Hashtable hs = new Hashtable();
        ColumnCardDto dto = new ColumnCardDto();
        hs.Add("@CreateUser", user);
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        string sql = string.Empty;
        var days = UtilHelper.month_day[month];
        if (type == FilterType.Year)
        {
            hs.Add("@year", year);
            sql =
                @"select  sum(OrderMoney) as  Value ,month(OrderTime) Label  from OrderInfo where OrderStatus in ('1','2')
                            and  CreateUser=@CreateUser  and year(OrderTime)=@year group by month(OrderTime)";
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            dto.XTypes = new string[months.Length];
            dto.YTypes = new Double[months.Length];
            for (int i = 0; i < months.Length; i++)
            {
                var item = months[i];
                var model = data.FirstOrDefault(d => item == Convert.ToInt32(d.Label));
                dto.XTypes[i] = $"{item}月";
                dto.YTypes[i] = 0;
                if (model != null)
                    dto.YTypes[i] = model.Value;
            }

            return dto;
        }
        if (type == FilterType.Month)
        {
            
            hs.Add("@year", year);
            hs.Add("@month",month);
            sql =
                @"select  sum(OrderMoney) as  Value ,day(OrderTime) Label  from OrderInfo where OrderStatus in ('1','2')
                            and  CreateUser=@CreateUser  
                          and year(OrderTime)=@year and month(OrderTime)=@month
                          group by day(OrderTime)";
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            dto.XTypes = new string[days];
            dto.YTypes = new Double[days];
            for (int i = 0; i < days; i++)
            {
                var item = i + 1;
                var model = data.FirstOrDefault(d => item == Convert.ToInt32(d.Label));
                dto.XTypes[i] = $"{month}月{item}日";
                dto.YTypes[i] = 0;
                if (model != null)
                    dto.YTypes[i] = model.Value;
            }

            return dto;
        }
        if (type == FilterType.Week)
        {
            int defaultDay = 7;
            start = UtilHelper.GetWeekRange().start;
            end=UtilHelper.GetWeekRange().end;
            hs.Add("@start",start);
            hs.Add("@end",end);
            var begin = Convert.ToDateTime(start);
            sql =
                @"select  sum(OrderMoney) as  Value ,OrderTime Label  from OrderInfo where OrderStatus in ('1','2')
                            and  CreateUser=@CreateUser  and  convert(datetime2,OrderTime)>=@start and convert(datetime2,OrderTime)<=@end
                                                                        group by OrderTime";
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            dto.XTypes = new string[defaultDay];
            dto.YTypes = new Double[defaultDay];
            for (int i = 0; i < defaultDay; i++)
            {
                var item = begin.AddDays(i);
                dto.XTypes[i] =item.ToString("yyyy/MM/dd");
                var model = data.FirstOrDefault(d => dto.XTypes[i] == d.Label);
                dto.YTypes[i] = 0;
                if (model != null)
                    dto.YTypes[i] = model.Value;
            }

            return dto;
        }

        if (type == FilterType.Rang)
        {
            int defaultDay =  (Convert.ToDateTime(end) - Convert.ToDateTime(start)).Days;;
            defaultDay += 1;
            hs.Add("@start",start);
            hs.Add("@end",end);
            var begin = Convert.ToDateTime(start);
            sql =
                @"select  sum(OrderMoney) as  Value ,OrderTime Label  from OrderInfo where OrderStatus in ('1','2')
                            and  CreateUser=@CreateUser  and  convert(datetime2,OrderTime)>=@start and convert(datetime2,OrderTime)<=@end
                                                                        group by OrderTime";
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            dto.XTypes = new string[defaultDay];
            dto.YTypes = new Double[defaultDay];
            for (int i = 0; i <defaultDay; i++)
            {
                var item = begin.AddDays(i);
                dto.XTypes[i] =item.ToString("yyyy/MM/dd");
                var model = data.FirstOrDefault(d => dto.XTypes[i] == d.Label);
                dto.YTypes[i] = 0;
                if (model != null)
                    dto.YTypes[i] = model.Value;
            }

        }

        return dto;
    }

    #endregion

    #region 进货数量

    private async Task<ColumnCardDto> GetPurashColumns(string user, FilterType type, string start, string end)
    {
        ColumnCardDto info = new ColumnCardDto();
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser",user);
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month; 
        if (type == FilterType.Year)
        {
            string sql = @"select  month(InOrderTime) Label, count(Id) Value from PurchaseInOrder
    where  CreateUser=@CreateUser and  year(InOrderTime)=@year
    group by  month(InOrderTime)"; 
            hs.Add("@year",year);
            info.XTypes = new string[12];
            info.YTypes = new double[12];
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            for (int i = 0; i < 12; i++)
            {
                var item = i + 1;
                info.XTypes[i] = $"{item}月";
                info.YTypes[i] = 0;
                var model = data.FirstOrDefault(d => d.Label == Convert.ToString(item));
                if (model != null)
                    info.YTypes[i] = model.Value;

            }
            
        }else if (type == FilterType.Month)
        {
            var days = UtilHelper.month_day[month];
            string sql = @"select  day(InOrderTime) Label, count(Id) Value from PurchaseInOrder
    where  CreateUser=@CreateUser and  year(InOrderTime)=@year and month(InOrderTime)=@month
    group by  day(InOrderTime)"; 
            hs.Add("@year",year);
            hs.Add("@month",month);
            info.XTypes = new string[days];
            info.YTypes = new double[days];
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            for (int i = 0; i < days; i++)
            {
                var item = i + 1;
                info.XTypes[i] = $"{month}月{item}日";
                info.YTypes[i] = 0;
                var model = data.FirstOrDefault(d => d.Label == Convert.ToString(item));
                if (model != null)
                    info.YTypes[i] = model.Value;

            }
        } else if (type == FilterType.Week)
        {
            var range = UtilHelper.GetWeekRange();
            int defaultDay = 7;
            var begin = Convert.ToDateTime(range.start);
            string sql = @"select InOrderTime as Label ,count(Id) as Value 
   from PurchaseInOrder where  CreateUser=@CreateUser
                and (  convert(datetime2,InOrderTime)>=@start and convert(datetime2,InOrderTime)<=@end) group by  InOrderTime";
            hs.Add("@start",Convert.ToDateTime(range.start));
            hs.Add("@end",Convert.ToDateTime(range.end));
            info.XTypes = new string[defaultDay];
            info.YTypes = new double[defaultDay];
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            for (int i = 0; i < defaultDay; i++)
            {
                var item = begin.AddDays(i);
                var time = item.ToString("yyyy/MM/dd");
                info.XTypes[i] = time;
                info.YTypes[i] = 0;
                var model = data.FirstOrDefault(d => d.Label ==time);
                if (model != null)
                    info.YTypes[i] = model.Value;
            }
        }
        else
        {  
            string sql = @"select InOrderTime as Label ,count(Id) as Value 
   from PurchaseInOrder where  CreateUser=@CreateUser
                and (  convert(datetime2,InOrderTime)>=@start and convert(datetime2,InOrderTime)<=@end) group by  InOrderTime";
            hs.Add("@start",start);
            hs.Add("@end",end);
            int defaultDay =  (Convert.ToDateTime(end) - Convert.ToDateTime(start)).Days;;
            defaultDay += 1;
            info.XTypes = new string[defaultDay];
            info.YTypes = new double[defaultDay];
            var data = await _sqlDapper.QueryAsync<ColumnsQueryDto>(sql, hs);
            var begin = Convert.ToDateTime(start);
            for (int i = 0; i < defaultDay; i++)
            {
                var item = begin.AddDays(i);
                var time = item.ToString("yyyy/MM/dd");
                info.XTypes[i] = time;
                info.YTypes[i] = 0;
                var model = data.FirstOrDefault(d => d.Label ==time);
                if (model != null)
                    info.YTypes[i] = model.Value;
            }
            
        }

        return info;
    }
    

    #endregion

    #region 订单统计

    public async Task<ReturnResult<OrderCardDto>> GetOrderColumns(ColumnsDto dto)
    {
        OrderCardDto info = new OrderCardDto();
        int year = DateTime.Now.Year;
        var user = _userHelper.LoginName;
        if (dto.FilterType == FilterType.Year)
        {
            Hashtable hs = new Hashtable();
            hs.Add("@CreateUser",user);
            var months = UtilHelper.GetMonths;
            //按年统计
            string sql =
                @"SELECT   NOWMONTH,OrderStatus as NOWSTATUS,COUNT(OrderStatus) NOWCOUNT FROM ( select year(convert(datetime2,OrderTime)) as NOWYEAR,
       month(convert(datetime2,OrderTime)) as NOWMONTH,
       OrderStatus from OrderInfo
                              where
                                  year(convert(datetime2,OrderTime))=@years
                              and month(convert(datetime2,OrderTime))
                              in (1,2,3,4,5,6,7,8,9,10,11,12)
                              and   CreateUser=@CreateUser
                              ) AS T
GROUP BY  OrderStatus,NOWMONTH";
            hs.Add("@years",year);
            var data = await _sqlDapper.QueryAsync<OrderQueryDto>(sql, hs);
            info.XTypes = new string[months.Length];
            info.DZFTypes = new int[months.Length]; //待支付
            info.YZFTypes = new int[months.Length];//已支付
            info.YWCTypes = new int[months.Length];
            info.ZFTypes = new int[months.Length];
            info.YQXTypes = new int[months.Length];
            int[] types = { 0, 1, 2, 3, 9 };
            for (int i = 0; i < months.Length; i++)
            {
                var item = i + 1;
                info.XTypes[i] = $"{item}月";
                var dzfModel = data.FirstOrDefault(d => d.NOWMONTH == item && d.NOWSTATUS == 0);
                info.DZFTypes[i] = 0;
                if (dzfModel != null)
                    info.DZFTypes[i] = dzfModel.NOWCOUNT;
                var yzfModel = data.FirstOrDefault(d => d.NOWMONTH == item && d.NOWSTATUS == 1);
                info.YZFTypes[i] = 0;
                if (yzfModel != null)
                    info.YZFTypes[i] = yzfModel.NOWCOUNT;
                var ywcModel =  data.FirstOrDefault(d => d.NOWMONTH == item && d.NOWSTATUS == 2);
                info.YWCTypes[i] = 0;
                if (ywcModel != null)
                    info.YWCTypes[i] = ywcModel.NOWCOUNT;
                var zfModel =  data.FirstOrDefault(d => d.NOWMONTH == item && d.NOWSTATUS == 3);
                info.ZFTypes[i] = 0;
                if (zfModel != null)
                    info.ZFTypes[i] = zfModel.NOWCOUNT;
                var qxModel = data.FirstOrDefault(d => d.NOWMONTH == item && d.NOWSTATUS == 9);
                info.YQXTypes[i] = 0;
                if(qxModel !=null) 
                    info.YQXTypes[i] =qxModel.NOWCOUNT;
            }

            


        }
        else if (dto.FilterType == FilterType.Month)
        {
            int month = DateTime.Now.Month;
            Hashtable hs = new Hashtable();
            hs.Add("@CreateUser", user);
            hs.Add("@years",DateTime.Now.Year);
            hs.Add("@months",month);
            //按月统计
            var days = UtilHelper.month_day[DateTime.Now.Month];
            string sql = @"SELECT   NOWMONTH,OrderStatus as NOWSTATUS,COUNT(OrderStatus) NOWCOUNT , NOWDAY FROM (select
                                                                                    month(convert(datetime2, OrderTime)) as NOWMONTH,
                                                                                    day(convert(datetime2,OrderTime)) as NOWDAY,
                                                                                    OrderStatus
                                                                             from OrderInfo
                                                                             where year(convert(datetime2, OrderTime)) = @years
                                                                               and month(convert(datetime2, OrderTime)) = @months
                                                                                and CreateUser=@CreateUser
                                                                            ) AS T
GROUP BY  OrderStatus,NOWMONTH,NOWDAY
";
            
            var data = await _sqlDapper.QueryAsync<OrderQueryDto>(sql, hs);
            info.XTypes = new string[days];
            info.DZFTypes = new int[days]; //待支付
            info.YZFTypes = new int[days];//已支付
            info.YWCTypes = new int[days];
            info.ZFTypes = new int[days];
            info.YQXTypes = new int[days];
            for (int i = 0; i < days; i++)
            {
                var item = i + 1;
                info.XTypes[i] = $"{month}月{item}日";
                var dzfModel = data.FirstOrDefault(d => d.NOWMONTH == month && d.NOWSTATUS == 0 && d.NOWDAY == item);
                info.DZFTypes[i] = 0;
                if (dzfModel != null)
                    info.DZFTypes[i] = dzfModel.NOWCOUNT;
                var yzfModel = data.FirstOrDefault(d => d.NOWMONTH == month && d.NOWSTATUS == 1&& d.NOWDAY == item);
                info.YZFTypes[i] = 0;
                if (yzfModel != null)
                    info.YZFTypes[i] = yzfModel.NOWCOUNT;
                var ywcModel =  data.FirstOrDefault(d => d.NOWMONTH == month && d.NOWSTATUS == 2&& d.NOWDAY == item);
                info.YWCTypes[i] = 0;
                if (ywcModel != null)
                    info.YWCTypes[i] = ywcModel.NOWCOUNT;
                var zfModel =  data.FirstOrDefault(d => d.NOWMONTH == month && d.NOWSTATUS == 3&& d.NOWDAY == item);
                info.ZFTypes[i] = 0;
                if (zfModel != null)
                    info.ZFTypes[i] = zfModel.NOWCOUNT;
                var qxModel = data.FirstOrDefault(d => d.NOWMONTH == month && d.NOWSTATUS == 9&& d.NOWDAY == item);
                info.YQXTypes[i] = 0;
                if(qxModel !=null) 
                    info.YQXTypes[i] =qxModel.NOWCOUNT;
            }

        }
        else if (dto.FilterType == FilterType.Week)
        {
            Hashtable hs = new Hashtable();
            hs.Add("@CreateUser", user);
            //按周统计
            int defaultDay = 7;
           var start = UtilHelper.GetWeekRange().start;
           var  end=UtilHelper.GetWeekRange().end;
            hs.Add("@start",start);
            hs.Add("@end",end);
            var begin = Convert.ToDateTime(start);
            string sql =
                @" select  t.OrderStatus,  count(t.OrderStatus) as NOWCOUNT,t.OrderTime  from ( select  OrderStatus,OrderTime from OrderInfo
  where
CreateUser=@CreateUser  and  convert(datetime2,OrderTime)>=@start and convert(datetime2,OrderTime)<=@end)
as t
group by  t.OrderTime,t.OrderStatus
";
            var data = await _sqlDapper.QueryAsync<OrderQueryWeekDto>(sql, hs);
            info.XTypes = new string[defaultDay];
            info.DZFTypes = new int[defaultDay]; //待支付
            info.YZFTypes = new int[defaultDay];//已支付
            info.YWCTypes = new int[defaultDay];
            info.ZFTypes = new int[defaultDay];
            info.YQXTypes = new int[defaultDay];
            for (int i = 0; i < defaultDay; i++)
            {
                var item = begin.AddDays(i);
                info.XTypes[i] =item.ToString("yyyy/MM/dd");
                
                var dzfModel = data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 0);
                info.DZFTypes[i] = 0;
                if (dzfModel != null)
                    info.DZFTypes[i] = dzfModel.NOWCOUNT;
                var yzfModel = data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 1);
                info.YZFTypes[i] = 0;
                if (yzfModel != null)
                    info.YZFTypes[i] = yzfModel.NOWCOUNT;
                var ywcModel =  data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 2);
                info.YWCTypes[i] = 0;
                if (ywcModel != null)
                    info.YWCTypes[i] = ywcModel.NOWCOUNT;
                var zfModel =  data.FirstOrDefault(d =>d.OrderTime == info.XTypes[i] && d.OrderStatus == 3);
                info.ZFTypes[i] = 0;
                if (zfModel != null)
                    info.ZFTypes[i] = zfModel.NOWCOUNT;
                var qxModel = data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 9);
                info.YQXTypes[i] = 0;
                if(qxModel !=null) 
                    info.YQXTypes[i] =qxModel.NOWCOUNT;
            }
        }
        else
        {
            //按照范围统计
            Hashtable hs = new Hashtable();
            hs.Add("@CreateUser", user);
            //按周统计
            int defaultDay =  (Convert.ToDateTime(dto.EndTime) - Convert.ToDateTime(dto.StartTime)).Days;;
            hs.Add("@start",dto.StartTime);
            hs.Add("@end",dto.EndTime);
            defaultDay += 1;
            var begin = Convert.ToDateTime(dto.StartTime);
            string sql =
                @" select  t.OrderStatus,  count(t.OrderStatus) as NOWCOUNT,t.OrderTime  from ( select  OrderStatus,OrderTime from OrderInfo
  where
CreateUser=@CreateUser  and  convert(datetime2,OrderTime)>=@start and convert(datetime2,OrderTime)<=@end)
as t
group by  t.OrderTime,t.OrderStatus
";
            var data = await _sqlDapper.QueryAsync<OrderQueryWeekDto>(sql, hs);
            info.XTypes = new string[defaultDay];
            info.DZFTypes = new int[defaultDay]; //待支付
            info.YZFTypes = new int[defaultDay];//已支付
            info.YWCTypes = new int[defaultDay];
            info.ZFTypes = new int[defaultDay];
            info.YQXTypes = new int[defaultDay];
            for (int i = 0; i < defaultDay; i++)
            {
                var item = begin.AddDays(i);
                info.XTypes[i] =item.ToString("yyyy/MM/dd");
                
                var dzfModel = data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 0);
                info.DZFTypes[i] = 0;
                if (dzfModel != null)
                    info.DZFTypes[i] = dzfModel.NOWCOUNT;
                var yzfModel = data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 1);
                info.YZFTypes[i] = 0;
                if (yzfModel != null)
                    info.YZFTypes[i] = yzfModel.NOWCOUNT;
                var ywcModel =  data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 2);
                info.YWCTypes[i] = 0;
                if (ywcModel != null)
                    info.YWCTypes[i] = ywcModel.NOWCOUNT;
                var zfModel =  data.FirstOrDefault(d =>d.OrderTime == info.XTypes[i] && d.OrderStatus == 3);
                info.ZFTypes[i] = 0;
                if (zfModel != null)
                    info.ZFTypes[i] = zfModel.NOWCOUNT;
                var qxModel = data.FirstOrDefault(d => d.OrderTime == info.XTypes[i] && d.OrderStatus == 9);
                info.YQXTypes[i] = 0;
                if(qxModel !=null) 
                    info.YQXTypes[i] =qxModel.NOWCOUNT;
            }
        }

        return new ReturnResult<OrderCardDto>(
            true, info, "获取订单统计成功");
    }
    

    #endregion

    #region 热销商品

    public async Task<ReturnResult> UpdateProdcutName()
    {
        string sql = "select  CustomerName,Id  from CustomerInfo;";
        var list = await _sqlDapper.QueryAsync<TestDto>(sql);
        foreach (var item in list)
        {
          var strs=  PinyinHelper.GetPinyinInitials(item.CustomerName);
          if (!string.IsNullOrWhiteSpace(strs))
              await _sqlDapper.UpdateAsync("Update CustomerInfo set NameSpell=@NameSpell where Id=@Id", new
              {
                  Id = item.Id,
                  NameSpell= strs
                  
              });

        }

        return new ReturnResult(true, null,"更新成功");
    }
    public async Task<ReturnResult<List<HeightProduct>>> GetHeightProduct(ColumnsDto dto)
    {
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser",_userHelper.LoginName);
        StringBuilder sb = new StringBuilder();
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        if (dto.FilterType == FilterType.Year)
        {
            sb.Append(" and year(a.OrderTime)=@Year ");
            hs.Add("@Year",year);
        } else if (dto.FilterType == FilterType.Month)
        {
            sb.Append(" and year(a.OrderTime)=@Year and month(a.OrderTime)=@month  ");
            hs.Add("@Year",year);
            hs.Add("@month", month);
            
        }else if (dto.FilterType == FilterType.Week)
        {
            var ranget = UtilHelper.GetWeekRange();
            sb.Append(" and (convert (datetime2,a.OrderTime)>=@start and   convert (datetime2,a.OrderTime)<=@end)");
            hs.Add("@start",Convert.ToDateTime(ranget.start));
            hs.Add("@end",Convert.ToDateTime(ranget.end));
        }else 
        {
            sb.Append(" and (convert (datetime2,a.OrderTime)>=@start and   convert (datetime2,a.OrderTime)<=@end)");
            hs.Add("@start",Convert.ToDateTime(dto.StartTime));
            hs.Add("@end",Convert.ToDateTime(dto.EndTime));
        }
            string sql = @"
select  top  10 b.ProductName, sum(b.OrderCount)  SellCount from  OrderInfo a join OrderInfoDetail b
on a.Id = b.OrderId  where  a.CreateUser=@CreateUser  "+sb.ToString()+" group by  b.ProductName order by  SellCount desc";
            var data = await _sqlDapper.QueryAsync<HeightProduct>(sql, hs);
            return new ReturnResult<List<HeightProduct>>(true, data, "获取热销商品成功");


    }

    #endregion

    #region  进货情况统计

    

    #endregion

    #region 卡片统计私有方法

    private async Task<string> GetProductStatic(string user)
    {
        string productSql =
            @"select  count(1) as ProductCount from ProductInfo  where Enable=1 and CreateUser=@CreateUser";
        int productCount = await _sqlDapper.QueryCountAsync(productSql, new { CreateUser = user });
        productSql =
            @"select count(1) as ProductCount  from ProductInfo  where Enable=1 and CreateUser=@CreateUser and  
                                           CreateTime between @Start and @End";
        string start = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        string end = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
        int dayCount =
            await _sqlDapper.QueryCountAsync(productSql, new { CreateUser = user, Start = start, End = end });
        return $"{productCount}-{dayCount}";
    }

    private async Task<string> GetSellPrices(string user)
    {
        var profiles = await getSellProfiles(user);
        var allPrice = profiles.Sum(d => d.OrderPrice);
        string start = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
        string end = DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
        var days = profiles.Where(d =>
                d.OrderTime >= Convert.ToDateTime(start) && d.OrderTime <= Convert.ToDateTime(end))
            .Sum(d => d.OrderPrice);
        return $"{Convert.ToString(allPrice)}-{Convert.ToString(days)}";
    }

    private async Task<string> GetProfilesPrices(string user)
    {
        var profiles = await getSellProfiles(user);
        var allPrice = profiles.Sum(d => d.OurProfile);
        string start = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
        string end = DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
        var days = profiles.Where(d =>
                d.OrderTime >= Convert.ToDateTime(start) && d.OrderTime <= Convert.ToDateTime(end))
            .Sum(d => d.OurProfile);
        return $"{Convert.ToString(allPrice)}-{Convert.ToString(days)}";
    }

    private async Task<string> GetOrderStatic(string user)
    {
        string countSql =
            @"select count (1) as ACount from  OrderInfo where OrderStatus in ('1','2') and CreateUser=@CreateUser";
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", user);
        int count = await _sqlDapper.QueryCountAsync(countSql, hs);

        string start = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
        string end = DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
        string daySql =
            @"select count (1) as ACount from  OrderInfo where OrderStatus in ('1','2') and CreateUser=@CreateUser
and   ( convert(datetime2,OrderTime)  between  @start and @end)";
        hs.Add("@start", Convert.ToDateTime(start));
        hs.Add("@end", Convert.ToDateTime(end));
        int dayCount = await _sqlDapper.QueryCountAsync(daySql, hs);
        return $"{count}-{dayCount}";
    }

    private async Task<List<SellProfileDto>> getSellProfiles(string user)
    {
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser", user);
        string sql = @"select  b.OrderPrice,
      ( (c.Purchase+ c.InitialCost)*b.OrderCount) OurMoney,
   ( b.OrderPrice- (c.Purchase+ c.InitialCost)*b.OrderCount ) OurProfile,
   a.OrderTime
from OrderInfo a join  OrderInfoDetail b
on a.Id =b.OrderId join ProductInfo c
    on b.ProductId = c.Id
            and c.ProductCode = b.ProductCode
where a.OrderStatus in ('1','2') and a.CreateUser=@CreateUser";

        var data = await _sqlDapper.QueryAsync<SellProfileDto>(sql, new { CreateUser = user });
        return data;
    }

    #endregion


    #region 系统信息

    public async Task<ReturnResult<SystemInfoDto>> GetSystem()
    {
        var config = _configuration.GetSection("LCPCConfig:SystemConfig")
            .Get<SystemInfoDto>();
        var lic = _configuration.GetSection("LCPCConfig:Licence").Value;
        config.HostName = Dns.GetHostName();
        config.SystemName = Environment.OSVersion.ToString();
        return new ReturnResult<SystemInfoDto>(true, config, "获取系统信息成功");
    }
    

    #endregion
}

public class TestDto
{
   
    public string CustomerName { get; set; }
    public string     Id { get; set; }
}