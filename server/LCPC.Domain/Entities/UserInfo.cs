namespace LCPC.Domain.Entities;

public class UserInfo:EntityBase
{
    public string UserName { get; set; }
    public string UserPass { get; set; }
    public string UserTel { get; set; }
    public string UserAddress { get; set; }
}