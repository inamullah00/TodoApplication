using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Interfaces.User;
using ToDo.Domin.Entities;

namespace ToDo.Application.Services.AuthServices
{
    public class AccountGenericService <T>: IAccountGenericService<T> where T : class
    {
        public readonly IAccountGenericRepository<T> _UserRepository;
        public AccountGenericService(IAccountGenericRepository<T> userRepository)
        {
            _UserRepository = userRepository;
        }

        public async Task<(T? user,bool isSuccessfull)> RegisterUserAsync(string Email, string Password)
        {
           return  await _UserRepository.AddAsync( Email, Password);
         
        }

        public async Task<(T? user, bool isSuccessfull)> LoginUserAsync(string Email, string Password)
        {
            return await _UserRepository.Loginasyn(Email,Password);
        }

        public async Task<bool> LogoutUserAsync()
        {
             await _UserRepository.LogoutAsync();
            return true;
        }

       
    }
}
