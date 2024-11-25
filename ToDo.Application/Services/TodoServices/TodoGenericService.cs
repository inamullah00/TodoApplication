using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Interfaces.TdoItem;
using ToDo.Domin.Entities;

namespace ToDo.Application.Services.TodoServices
{
    public class TodoGenericService<T> : ITodoGenericService<T> where T : class
    {
        #region Fields and Constructor

        private readonly ITodoGenericRepository<T> _todoGenericRepository;

        public TodoGenericService(ITodoGenericRepository<T> todoGenericRepository)
        {
            _todoGenericRepository = todoGenericRepository;
        }
        #endregion

        #region AddAsync Method
        public async Task<bool> AddAsync(T item)
        {
           return await _todoGenericRepository.AddAsync(item);

        }
        #endregion

        #region DeleteAsync Method
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _todoGenericRepository.DeleteAsync(id);
        }
        #endregion

        #region GetByIdAsync Method
        public async Task<List<T>> GetAllAsync()
        {
          return await _todoGenericRepository.GetAllAsync();
        }
        #endregion

        #region GetByIdAsync Method
        public async Task<T?> GetByIdAsync(Guid id , Expression<Func<T, object>>? includeExpressions)
        {
           return await _todoGenericRepository.GetTByIdAsync(id, includeExpressions);
        }
        #endregion
       
        #region SaveAsync Method (Not Implemented) 
        public async Task<bool> UpdateAsync(Guid id, T todoItem)
        {
            return await _todoGenericRepository.UpdateAsync(id, todoItem);
        }
        #endregion

        #region ShareAsync Method (Not Implemented)
        public Task ShareAsync()
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
