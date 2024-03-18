namespace LCPC.Domain.Entities;

public class UpdateLog:EntityBase
{
    public string Version { get; set; }
    public string ToUser { get; set; }
}