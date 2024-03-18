
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace LCPC.Infrastructure.Repositories
{
  public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
  {
    private readonly IServiceProvider _serviceProvider;
    public UserInfoRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }
    public async Task<UserInfo> GetUserInfoByName(string name)
    {
     
      
      
      var user = await base.FindEntity(x => x.UserName.Equals(name));
      return user;
    }
  }
}