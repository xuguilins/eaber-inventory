using Microsoft.AspNetCore.Http;

namespace LCPC.Domain.Services;

public class UserHelper : IScopeDependecy
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserHelper(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }

    public string LoginName
    {
        get
        {
            var principal = _httpContextAccessor.HttpContext.User;
            if (principal == null)
                return string.Empty;
            var cliams = principal.Claims.ToList();
            if (!cliams.Any())
                return string.Empty;
            var user = cliams.FirstOrDefault(d => d.Type == "userName");
            if (user == null)
                return string.Empty;
            return user.Value;
        }
    }
}