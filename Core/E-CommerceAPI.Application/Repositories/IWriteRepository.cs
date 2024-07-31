using E_CommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> models);

        bool Remove(T model);
        Task<bool> RemoveAsync(string id);
        bool RemoveRange(List<T> models);

        bool Update(T model);

        Task<int> SaveAsync();
    }
}
