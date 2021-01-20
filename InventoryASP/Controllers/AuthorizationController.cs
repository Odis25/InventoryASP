using InventoryApp.Services.Interfaces;
using InventoryApp.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public string ReturnUrl { get; set; }

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return PartialView(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]       
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authorizationService.LoginAsync(model.LoginName, model.Password, model.RememberMe);

                    if (!result)
                    {
                        ModelState.AddModelError("", "Неправильный логин или пароль");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _authorizationService.Logout();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
