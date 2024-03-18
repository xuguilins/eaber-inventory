using LCPC.Share.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace LCPC.Domain.Services;

public class TokenManager : ITokenManager
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    public TokenManager(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _contextAccessor = httpContextAccessor;
    }

    public string CreateJsonWebRefashToken(string userName)
     => createAccess_Token(userName, 43200);
    public string CreateJsonWebToken(string userName)
     => createAccess_Token(userName);

    public  async Task<ReturnResult>  RefrashToken()
    {
        ReturnResult result = new ReturnResult(true);
        // 解析当前token 
        string oldToken = _contextAccessor.HttpContext.Request.Headers.Authorization.ToString();
        string newToken = _contextAccessor.HttpContext.Request.Headers["AuthorizationX"].ToString();
        if (newToken.StartsWith("Bearer"))
            newToken = newToken.Substring(7);
        var oldJwt = DecodeToken(oldToken);
        var newJwt = DecodeToken(newToken);
        if (oldJwt.Count != newJwt.Count)
            throw new Exception("无法自动刷新token,签名不一致！");
        var oldCliams = oldJwt.Claims.ToList();
        var newCliams = newJwt.Claims.ToList();
        var oldUser = oldCliams.FirstOrDefault(d => d.Type == "userName");
        if (oldUser == null)
            throw new Exception("token已被损坏,无法解析");
        var newUser =  newCliams.FirstOrDefault(d => d.Type == "userName");
        if (newUser == null)
            throw new Exception("refeshtoken 已被损坏,无法解析");
        if(oldUser.Value !=  newUser.Value)
            throw new Exception("token已被串改,无法解析");

        string Access_Token = createAccess_Token(newUser.Value);

        result.Data = new
        {
            Access_Token = Access_Token,
            Refresh_Token = newToken
        };
        result.Message = "自动刷新身份成功";
        return await Task.FromResult(result);

    }

    private JwtPayload DecodeToken(string jwtStr)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        if (jwtStr.StartsWith("Bearer"))
            jwtStr = jwtStr.Substring(7);
        JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
        return jwtToken.Payload;
    }
    private string createAccess_Token(string userName, int expretime = 60)
    {
        DateTime time =DateTime.Now;
        var jwtOption = _configuration.GetSection("LCPCConfig").Get<LCPCConfig>();
        List<Claim> claims = new List<Claim>
        {
            new Claim("userName", userName),
            new Claim("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(expretime).ToString(CultureInfo.CurrentCulture)),
            new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(time).ToUnixTimeSeconds()}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(time.AddMinutes(expretime)).ToUnixTimeSeconds()}"),
            new Claim(JwtRegisteredClaimNames.Aud, jwtOption.JWT.Audience),
            new Claim(JwtRegisteredClaimNames.Iss, jwtOption.JWT.Issuer),
        };
        //密钥处理，key和加密算法
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.JWT.Secret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            claims: claims,
            signingCredentials: cred,
            notBefore: time,
            expires:time.AddMinutes(expretime)
        );
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwt);
        return token;
    }
}