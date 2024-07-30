using E_CommerceAPI.Application.Abstractions;
using E_CommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts() => new()
        {
            new(){ Id =  Guid.NewGuid(), Name = "Product 1", Price= 100, Stock = 15 },
            new(){ Id =  Guid.NewGuid(), Name = "Product 2", Price= 200, Stock = 19 },
            new(){ Id =  Guid.NewGuid(), Name = "Product 3", Price= 300, Stock = 87 }
        };
    }
}
