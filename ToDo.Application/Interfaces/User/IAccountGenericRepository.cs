using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domin.Entities;

namespace ToDo.Application.Interfaces.User
{
    public interface IAccountGenericRepository<T> where T : class
    {
        
   
        Task<(T? user,bool isSuccessfull)> AddAsync(string Email, string Password);
        Task<(T? user, bool isSuccessfull)> Loginasyn(string Email, string Password);
        Task<bool> LogoutAsync();
    }
}
