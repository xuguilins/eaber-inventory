using System.Text;
using Microsoft.AspNetCore.Http;

namespace LCPC.Share;

public static class UtilHelper
{
     public static Dictionary<int, int> month_day = new Dictionary<int, int>()
     {
          { 1,31},
          {2,( (DateTime.Now.Year % 400 == 0 || 
                DateTime.Now.Year % 4 == 0 && 
                DateTime.Now.Year % 100 != 0) ? 29: 28)},
          {3,31},
          {4,30},
          {5,31},
          {6,30},
          {7,31},
          {8,31},
          {9,30},
          {10,31},
          {11,30},
          {12,31}
          
     };
     public static int[] GetMonths
     {
          get
          {
               List<int> list = new List<int>();
               for (int i = 1; i <= 12; i++)
               {
                list.Add(i);    
               }

               return list.ToArray();
          }
     }

     public static (string start, string end) GetWeekRange()
     {
          var time = Convert.ToInt32( DateTime.Now.DayOfWeek);
          if (time == 0)
               time = 7;
          var nowTime = DateTime.Now;
          int offset = 7 - time;
          var endValue = nowTime.AddDays(offset);
          var endTime = nowTime.AddDays(offset).ToString("yyyy/MM/dd 23:59:59");
          var startTime = endValue.AddDays(-6).ToString("yyyy/MM/dd 00:00:00");
          return
          (
             startTime,
             endTime
          );
     }


     
     public static string getNewId()
     {
          var guid = Guid.NewGuid().ToString("N").ToUpper();
          var bytes = Encoding.UTF8.GetBytes(guid);
          Array.Reverse(bytes);
          string result = string.Empty;
          for (int j = 0; j < 2; j++)
          {
               result += bytes[j];
          }
          var uid = guid.Substring(0, guid.Length - 15);
          return result + uid;
     }

     public static DateTime GetNetworkTime()
     {
          return DateTime.Now;
          using (HttpClient client = new HttpClient())
          {
               var response = client.GetAsync("https://www.baidu.com").Result;
               var header = response.Headers.Date;
               if (header != null)
                    return header.Value.Date;
               return DateTime.Now;
          }
     }
}