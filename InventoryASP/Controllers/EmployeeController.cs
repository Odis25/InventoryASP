using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryASP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;
        private readonly ICheckout _checkoutService;
        private readonly IDepartment _departmentService;
        private readonly IPosition _positionService;


        public EmployeeController(IEmployee employeeService, ICheckout checkoutService, IDepartment departmentService, IPosition positionService)
        {
            _employeeService = employeeService;
            _checkoutService = checkoutService;
            _departmentService = departmentService;
            _positionService = positionService;
        }

        // Список сотрудников
        public IActionResult Index()
        {
            var employees = _employeeService.GetAll();

            var listingResult = employees.Select(employee => new EmployeeListingModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
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

        // Форма добавления нового сотрудника
        public IActionResult NewEmployee()
        {
            var departments = _departmentService.GetAll();
            var positions = _positionService.GetAll();

            ViewBag.Departments = departments;
            ViewBag.Positions = positions;

            return PartialView();
        }

        // Добавить нового сотрудника
        [HttpPost]
        public IActionResult NewEmployee(NewEmployeeModel model)
        {
            var employee = BuildNewEmployee(model);
            _employeeService.Add(employee);

            return RedirectToAction("Index", "Employee");
        }

        // Удалить сотрудника
        public IActionResult DeleteEmployee(IEnumerable<int> idList)
        {
            if (idList.Any())
                _employeeService.Delete(idList.ToArray());
            
            return RedirectToAction("Index", "Employee");
        }

        // Добавить устройство сотруднику
        [HttpPost]
        public IActionResult CheckOutDevice(AvalibleDevicesModel model)
        {
            var idList = model.Devices.Where(d => d.IsSelected).Select(d => d.Id).ToArray();
            _checkoutService.CheckOutDevice(model.EmployeeId, idList);

            return RedirectToAction("Details", "Employee", new { id = model.EmployeeId });
        }

        // Забрать устройство у сотрудника
        [HttpPost]
        public IActionResult CheckInDevice(EmployeeDetailsModel model)
        {
            var idList = model.Checkouts.Where(c => c.IsSelected)
                .Select(c => c.Checkout.Device.Id);

            _checkoutService.CheckInDevice(idList);

            return RedirectToAction("Details", new { id = model.Id });
        }

        // Детальная информация о сотруднике
        public IActionResult Details(int id)
        {
            var employee = _employeeService.GetById(id);

            var checkouts = _checkoutService.GetByEmployeeId(id)
                .Select(c => new CheckoutModel { Checkout = c }).ToList();

            var checkoutsHistory = _checkoutService.GetEmployeeHistory(id).ToList();

            var model = new EmployeeDetailsModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Department = employee.Department.Name,
                Position = employee.Position.Name,
                Checkouts = checkouts,
                History = checkoutsHistory
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
                Department = department,
                Status = "Available"
            };
        }
    }
}
