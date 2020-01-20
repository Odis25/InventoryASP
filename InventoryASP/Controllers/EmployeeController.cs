using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Text;

namespace InventoryASP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;
        private readonly IDepartment _departmentService;
        

        public EmployeeController(IEmployee employeeService, IDepartment departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
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

        public IActionResult NewEmployee()
        {
            var departments = _departmentService.GetAll();

            ViewBag.Departments = departments;

            var model = new NewEmployeeModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddNewEmployee()
        {

            return RedirectToAction("Index", "Employee");
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
