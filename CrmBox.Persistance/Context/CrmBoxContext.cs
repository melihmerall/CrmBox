using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmBox.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrmBox.Persistance.Context
{
    public class CrmBoxContext : DbContext
    {
        readonly IConfiguration _configuration;

        public CrmBoxContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Main"));
        }
        public DbSet<Customer>? Customers { get; set; }
    }
    
}
