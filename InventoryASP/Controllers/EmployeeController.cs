using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

namespace InventoryASP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;

        public EmployeeController(IEmployee employeeService, ICheckout checkoutService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            var employees = _employeeService.GetAll();

            var listingResult = employees.Select(employee => new EmployeeListingModel
            {
                Id = employee.Id,
                FullName = GetFullName(employee),
                Position = employee.Position.Name,
                Department = employee.Department.Name,
                Devices = _employeeService.GetHoldedDevices(employee.Id)
            });

            var model = new EmployeeIndexModel
            {
                Employees = listingResult
            };

            return View(model);
        }

        // Получаем фамилию и инициалы
        private string GetFullName(Employee employee)
        {
            var fullName = new StringBuilder()
                .Append(employee.LastName)
                .Append(" ")
                .Append(employee.Name.First())
                .Append(".")
                .Append(employee.Patronymic.First())
                .Append(".")
                .ToString();
            return fullName;
        }
    }
}
