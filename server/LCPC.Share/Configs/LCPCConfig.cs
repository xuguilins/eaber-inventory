namespace LCPC.Share.Configs;

public class LCPCConfig
{
    public JWTConfig JWT { get; set; }
    public SystemConfig SystemConfig { get; set; }
    public string CORS { get; set; }
}

public class JWTConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
}

public class SystemConfig
{
    public string ProductName { get; set; }
    public string ProductVersion { get; set; }
    public string ProductType { get; set; }

    public string AuthTarget { get; set; }
    public string BackService { get; set; }
    public string FrontService { get; set; }

    public string Deployment { get; set; }
    public string DatabaseType { get; set; }
    public string DatabaseVersion { get; set; }
}