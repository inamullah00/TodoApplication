using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Interfaces.User;
using ToDo.Domin.Entities;


namespace ToDo.Infrastructure.Persistance.Repository
{
    public class AccountGenericRepository<T> : IAccountGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountGenericRepository(
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(T? user,bool isSuccessfull)> AddAsync(string Email, string Password)
        {
            // Create an instance of T (which should be ApplicationUser or a subclass of it)
            var user = Activator.CreateInstance<T>();

            // Ensure that T is actually of type ApplicationUser (or its subclass) to access properties
            if (user is ApplicationUser applicationUser)
            {
                // Set the UserName and Email properties
                applicationUser.UserName = Email;
                applicationUser.Email = Email;

                // Create the user in the database
                var result = await _userManager.CreateAsync(applicationUser, Password);

                if (result.Succeeded)
                {
                    return (user, true);
                }
            }

            // If creation fails or T is not of type ApplicationUser, return null and false
            return (null, false);
        }


        public async Task<(T? user, bool isSuccessfull)> Loginasyn(string Email, string Password)
        {
            // Attempt to find the user by email
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                // User not found
                return (null, false);
            }

            // Check if T is of type ApplicationUser or its subclass
            if (user is T ApplicationUser)
            {
                // Attempt to sign in the user
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return (ApplicationUser, true);
                }
            }

            // Sign-in failed or casting to T failed
            return (null, false);
        }

        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
   
    }
}
