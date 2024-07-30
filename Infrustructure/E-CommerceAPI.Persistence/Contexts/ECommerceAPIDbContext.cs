using E_CommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Persistence.Contexts
{
    public class ECommerceAPIDbContext : DbContext
    {
        public ECommerceAPIDbContext(DbContextOptions<ECommerceAPIDbContext> options) : base(options)
        {}

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
