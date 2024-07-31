using E_CommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetWehere(Expression<Func<T, bool>> expression);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression); //firstOrDefaultAsync()

        Task<T> GetByIdAsync(string id);
    }
}
