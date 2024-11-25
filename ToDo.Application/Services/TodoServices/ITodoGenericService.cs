using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domin.Entities;

namespace ToDo.Application.Services.TodoServices
{
    public interface ITodoGenericService<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id, Expression<Func<T, object>>? includeExpressions);
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(Guid id , T Item);
        Task<bool> DeleteAsync(Guid id);
        Task ShareAsync();
    
    }
}
