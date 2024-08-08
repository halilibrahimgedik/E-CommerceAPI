using F = E_CommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceAPI.Application.Repositories;
using E_CommerceAPI.Persistence.Contexts;

namespace E_CommerceAPI.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<F::File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }



    }
}
