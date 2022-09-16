using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CrmBox.Persistance.Context;

public class CrmBoxIdentityContext : IdentityDbContext<AppUser, AppRole, int>
{
    readonly IConfiguration _configuration;
    public DbSet<ChatRoom> Rooms { get; set; }
    public DbSet<ChatMessage> Messages { get; set; }
    
    public CrmBoxIdentityContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Identity"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());






    }


}