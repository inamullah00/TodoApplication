using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using ToDo.Application.Services.AuthServices;
using ToDo.Domin.Entities;
using ToDo.Presentation.ViewModels.Account;
using ToDo.Presentation.ViewModels.Registration;

namespace ToDo.Presentation.Controllers.AuthControllers
{
    public class AuthController : Controller
    {

        public readonly IAccountGenericService<ApplicationUser> _RegistrationService;
        public AuthController(IAccountGenericService<ApplicationUser> RegistrationService)
        {
            _RegistrationService = RegistrationService;
        }

        [HttpGet]
        [Route("/Signup")]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [Route("/Signup")]
        public async Task<ActionResult> Signup(RegistrationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model); 
                }
                var (user, isSuccessful) = await _RegistrationService.RegisterUserAsync(model.Email,model.Password);
          
                if (isSuccessful)
                {
                    return RedirectToAction("Login", "Auth");
                }
            
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model); 
            }

        }


        #region Login

        [HttpGet]
        [Route("/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                var (user, isSuccessful) = await _RegistrationService.LoginUserAsync(model.Email, model.Password);

                if (isSuccessful)
                {
                    return RedirectToAction("TodoItems", "Home");
                }
                ModelState.AddModelError("", "Invalid email or password. Please try again.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model); 
            }
        }

        #endregion

        #region Logout

        [HttpGet]
        [Route("Logout")]
        public async Task<ActionResult> Logout(LoginViewModel model)
        {
            try
            {
                 await _RegistrationService.LogoutUserAsync();
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Auth");
            }
        }
        #endregion

    }
}