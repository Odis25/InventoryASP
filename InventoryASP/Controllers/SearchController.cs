using InventoryApp.Services.Interfaces;
using InventoryApp.Models.Device;
using InventoryApp.Models.Employee;
using InventoryApp.Models.Search;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _search;

        public SearchController(ISearchService search, ICheckoutService checkouts)
        {
            _search = search;
        }

        public async Task<IActionResult> FindAsync(string searchQuery)
        {
            var employees = await _search.FindEmployeesAsync(searchQuery);

            var devices = await _search.FindDevicesAsync(searchQuery);

            var model = new SearchResultModel
            {
                Devices = devices,
                Employees = employees,
                SearchQuery = searchQuery
            };
            return View("SearchResult", model);
        }

        public async Task<IActionResult> FindEmployeeAsync(string searchQuery)
        {
            var employees = await _search.FindEmployeesAsync(searchQuery);

            var model = new EmployeeIndexModel
            {
                Employees = employees
            };

            return View("~/Views/Employee/Index.cshtml", model);
        }

        public async Task<IActionResult> FindDeviceAsync(string searchQuery)
        {
            var devices = await _search.FindDevicesAsync(searchQuery);

            var model = new DeviceIndexModel
            {
                Devices = devices,
            };

            return View("~/Views/Device/Index.cshtml", model);
        }        
    }
}
