using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        Task<string> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
