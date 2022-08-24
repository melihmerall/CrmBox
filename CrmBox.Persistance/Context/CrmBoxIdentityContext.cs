using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrmBox.Persistance.Context;

public class CrmBoxIdentityContext : IdentityDbContext<AppUser, AppRole, int>
{
    readonly IConfiguration _configuration;

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

        builder.Entity<AppUser>().Ignore(c => c.PhoneNumber)
            .Ignore(c => c.PhoneNumberConfirmed)
            .Ignore(c => c.PhoneNumber)
           
            .Ignore(c => c.EmailConfirmed)
            .Ignore(c => c.TwoFactorEnabled);
        
    }
   

}