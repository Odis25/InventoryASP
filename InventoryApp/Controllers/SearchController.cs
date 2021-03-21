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
        private readonly IEmployeeService _employeeService;
        private readonly IDeviceService _deviceService;

        public SearchController(IEmployeeService employeeService, IDeviceService deviceService)
        {
            _employeeService = employeeService;
            _deviceService = deviceService;
        }

        public async Task<IActionResult> IndexAsync(string searchPattern)
        {
            var employees = await _employeeService.GetEmployeesAsync(searchPattern);

            var devices = await _deviceService.GetDevicesAsync(searchPattern);

            var model = new SearchResultModel
            {
                Devices = devices,
                Employees = employees,
                SearchQuery = searchPattern
            };
            return View("SearchResult", model);
        }      
    }
}
