using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Find(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task Delete(T entity);
    }
}
