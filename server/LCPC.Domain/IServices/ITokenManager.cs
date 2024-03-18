namespace LCPC.Domain.IServices;

public interface ITokenManager:IScopeDependecy
{
    string CreateJsonWebToken(string userName);
    
    string CreateJsonWebRefashToken(string userName);

    Task<ReturnResult> RefrashToken();


} 