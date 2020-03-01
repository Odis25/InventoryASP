using InventoryAppData.Models;
using InventoryASP.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public string ReturnUrl { get; set; }

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginModel { ReturnUrl = returnUrl };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LoginName, model.Password, model.RememberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            // Удаляем аутентификационные куки
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
                return LocalRedirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> ManageUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var model = new ManageUserModel
            {
                UserId = userId,
                UserName = user.Name
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ManageUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

                if (result.Succeeded)
                    return LocalRedirect("~/Home/Index");
            }

            return PartialView("ManageUser", model);
        }
    }
}
