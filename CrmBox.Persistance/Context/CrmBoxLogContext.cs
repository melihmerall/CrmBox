using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrmBox.Persistance.Context;

public class CrmBoxLogContext : DbContext
{
    readonly IConfiguration _configuration;

    public CrmBoxLogContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Log"));
    }
}