using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LCPC.Infrastructure.Cores;


public class AdminFactory:IDesignTimeDbContextFactory<AdminDbContext>
{
    public AdminDbContext CreateDbContext(string[] args)
    {

        var options = new DbContextOptionsBuilder<AdminDbContext>();
        options.UseSqlServer("Data Source=120.26.92.18;Initial Catalog=LCPCBACK;User Id=sa;Password=070103;TrustServerCertificate=true");
        var optionData = options.Options;
        return new AdminDbContext(optionData);
    }
}