using System;
namespace LCPC.Domain.IRepositories
{
	public interface IUserInfoRepository:IRepository<UserInfo>
	{
	    Task<UserInfo> GetUserInfoByName(string name);
	}
}

