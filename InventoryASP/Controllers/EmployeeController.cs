using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryASP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;
        private readonly IDepartment _departmentService;
        private readonly IPosition _positionService;


        public EmployeeController(IEmployee employeeService, IDepartment departmentService, IPosition positionService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _positionService = positionService;
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
            var positions = _positionService.GetAll();

            ViewBag.Departments = departments;
            ViewBag.Positions = positions;

            var model = new NewEmployeeModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(NewEmployeeModel model)
        {
            var employee = BuildNewEmployee(model);
            await _employeeService.Add(employee);

            return RedirectToAction("Index", "Employee");
        }

        [HttpPost]
        public async Task<IActionResult> AddDeviceToEmployee(AddDeviceModel model)
        {
            var idList = model.Devices.Where(d => d.IsSelected == true)?.Select(d => d.Id);

            await _employeeService.GiveDevices(idList, model.EmployeeId);

            return RedirectToAction("Details", "Employee", new { id = model.EmployeeId });
        }

        public IActionResult Details(int id)
        {
            var employee = _employeeService.GetById(id);
            var history = _employeeService.GetEmployeeHistory(employee.Id);

            var model = new EmployeeDetailsModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Department = employee.Department.Name,
                Position = employee.Position.Name,
                Checkouts = employee.Checkouts,
                History = history               
            };

            return View(model);
        }

        // Создаем модель сотрудника
        private Employee BuildNewEmployee(NewEmployeeModel model)
        {
            var department = _departmentService.GetDepartment(model.Department.Id);
            var position = _positionService.GetPosition(model.Position.Id);

            return new Employee
            {
                Name = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Position = position,
                Department = department
            };
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
