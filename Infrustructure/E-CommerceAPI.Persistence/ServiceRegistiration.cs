using E_CommerceAPI.Application.Abstractions;
using E_CommerceAPI.Persistence.Concretes;
using E_CommerceAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Persistence
{
    public static class ServiceRegistiration
    {
        public static void AddPersistenceServices(this IServiceCollection services,IConfiguration conf)
        {
            services.AddDbContext<ECommerceAPIDbContext>(options =>
            {
                options.UseNpgsql(conf.GetConnectionString("PostgreSql"));
            });

            services.AddSingleton<IProductService, ProductService>();
        }
    }
}
