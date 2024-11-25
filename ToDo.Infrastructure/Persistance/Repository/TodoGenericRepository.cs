using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Interfaces.TdoItem;
using ToDo.Domin.Entities;

namespace ToDo.Infrastructure.Persistance.Repository
{
    public class TodoGenericRepository<T> : ITodoGenericRepository<T> where T :class
    {
        #region Fields and Constructor

        private readonly ApplicationDbContext _dbContext;

        public TodoGenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region AddAsync Method
        public async Task<bool> AddAsync(T item)
        {
        
            var res = await _dbContext.Set<T>().AddAsync(item);
            if (res != null && await _dbContext.SaveChangesAsync() > 0) { 
                return true;
            }
           return false ;
          
        }
        #endregion

        #region AddAsync Method
        public async Task<bool> DeleteAsync(Guid id)
        {
            var todoItem = await _dbContext.Set<T>().FindAsync(id);
            if (todoItem == null)
            {
                return false;
            }

            _dbContext.Set<T>().Remove(todoItem);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region GetAllAsync Method
        public async Task<List<T?>> GetAllAsync( Expression<Func<T, object>>? includeExpression = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();  

           
            if (includeExpression != null)
            {
                query = query.Include(includeExpression); 
            }

            return await query.ToListAsync(); 
        }
        #endregion

        #region GetTByIdAsync Method
        public async Task<T?> GetTByIdAsync(Guid id, Expression<Func<T, object>>? includeExpression = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

          
            if (includeExpression != null)
            {
                query = query.Include(includeExpression);
            }

           
            var idProperty = typeof(T).GetProperty("Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException("The type does not have an 'Id' property.");
            }

            
            var result = await query.FirstOrDefaultAsync(item => EF.Property<Guid>(item, "Id") == id);
            
           return result;
        }
        #endregion

        #region UpdateAsync Method
        public async Task<bool> UpdateAsync(Guid id, T todoItem)
        {
            // Use reflection to check for the Id property
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("The provided type does not have an Id property.");
            }

            // Use AsEnumerable to move the query to memory and perform the comparison
            var existingItem =  _dbContext.Set<T>()
                                                .AsEnumerable() // Moves query to client-side
                                                .FirstOrDefault(i => (Guid)idProperty.GetValue(i) == id);

            if (existingItem == null)
            {
                return false;
            }

            // Update the properties of the existing item
            _dbContext.Entry(existingItem).CurrentValues.SetValues(todoItem);

            // Save the changes
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}
