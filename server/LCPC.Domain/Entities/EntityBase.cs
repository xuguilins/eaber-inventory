using System.Text;

namespace LCPC.Domain.Entities;

public abstract class EntityBase
{
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string CreateUser { get; set; }="aaa";
    public string Remark { get; set; }
    public bool Enable { get; set; } = true;
    public string Id { get; set; }= getNewId();
    private static string getNewId()
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
}