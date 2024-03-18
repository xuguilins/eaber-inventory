namespace LCPC.Domain.QueriesDtos;

public record PayInfoDto
{
    public string Id { get; set; }
    public string PayName { get; set; }
    public string Remark { get; set; }
    public bool Enable { get; set; }
}

public class EnablePayInfoDto
{
    public string Id { get; set; }
    public string PayName { get; set; }
}