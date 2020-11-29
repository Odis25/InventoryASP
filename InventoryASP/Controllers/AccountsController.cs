using InventoryAppServices.Interfaces;
using InventoryASP.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryASP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public string ReturnUrl { get; set; }

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAccountsAsync();

            var model = new IndexModel { Accounts = accounts.ToList() };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(IndexModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var needRedirect = await _accountService.SaveChanges(model.Accounts);

            if (needRedirect)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
