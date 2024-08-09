using E_CommerceAPI.Application.Abstraction.Storage;
using E_CommerceAPI.Infrustructure.Enums;
using E_CommerceAPI.Infrustructure.Services;
using E_CommerceAPI.Infrustructure.Services.Storage;
using E_CommerceAPI.Infrustructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Infrustructure
{
    public static class ServiceRegistiration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage // T class ve IStorage'dan türemiş olmalı
        {
            services.AddScoped<IStorage, T>();
        }

        // bu extension metodun amacı yukarıdaki ile aynı sadece enum üzerinden değer vermek için böyle yaptık
        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    //services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    //services.AddScoped<IStorage, AWSStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

       
    }
}
