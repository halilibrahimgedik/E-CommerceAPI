using F = E_CommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceAPI.Application.Repositories;
using E_CommerceAPI.Persistence.Contexts;

namespace E_CommerceAPI.Persistence.Repositories.File
{
    public class FileWriteRepository : WriteRepository<F::File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }


    }
}
