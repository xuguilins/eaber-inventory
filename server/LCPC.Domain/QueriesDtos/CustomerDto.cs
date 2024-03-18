namespace LCPC.Domain.QueriesDtos;

public class CustomerDto
{
    public string Id { get; set; }
    public string CuCode { get; set; }
    public string CuName { get; set; }
    public string CuUser { get; set; }
    public string CuTel { get; set; }
    public string CuAddress { get; set; }
    public string CuPhone { get; set; }
  
    public string Remark { get; set; }
    public bool Enable { get; set; }
}