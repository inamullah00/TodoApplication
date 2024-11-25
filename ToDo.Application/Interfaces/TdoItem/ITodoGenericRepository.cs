using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domin.Entities;

namespace ToDo.Application.Interfaces.TdoItem
{
    public interface ITodoGenericRepository<T> where T : class
    {
        Task<List<T?>> GetAllAsync(Expression<Func<T, object>>? includeExpression = null);
        Task<T?> GetTByIdAsync(Guid id, Expression<Func<T, object>>? includeExpression = null);
        Task<bool> AddAsync(T todoItem);
        Task<bool> UpdateAsync(Guid id, T todoItem);
        Task<bool> DeleteAsync(Guid id);




    }
}
