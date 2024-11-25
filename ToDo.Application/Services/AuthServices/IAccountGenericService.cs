
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domin.Entities;


namespace ToDo.Application.Services.AuthServices
{
    public interface IAccountGenericService<T> where T : class
    {

        // Method to create a new user
        Task<(T? user,bool isSuccessfull)> RegisterUserAsync(string Email, string Password);
        Task<bool> LogoutUserAsync();
        Task<(T? user, bool isSuccessfull)> LoginUserAsync(string Email, string Password);



    }
}
