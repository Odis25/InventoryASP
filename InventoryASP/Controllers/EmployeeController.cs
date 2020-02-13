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
        private readonly IEmployee _employees;
        private readonly ICheckout _checkouts;

        public EmployeeController(IEmployee employees, ICheckout checkouts)
        {
            _employees = employees;
            _checkouts = checkouts;
        }

        // Список сотрудников
        public IActionResult Index()
        {
            var employees = _employees.GetAll();

            var listingResult = employees.Select(employee => new EmployeeListingModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Position = employee.Position.Name,
                Department = employee.Department.Name,
                Checkouts = employee.Checkouts
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
            var employee = new Employee
            {
                Name = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Position = model.Position,
                Department = model.Department,
            };

            _employees.Add(employee);

            return RedirectToAction("Index", "Employee");
        }

        // Удалить сотрудника
        public IActionResult DeleteEmployee(int id)
        {
            ViewBag.Id = id;
            return PartialView();
        }

        // Удалить сотрудника
        [HttpPost]
        public IActionResult DeleteEmployeePost(int id)
        {
            _employees.Delete(id);

            return RedirectToAction("Index", "Employee");
        }

        // Добавить устройство сотруднику
        [HttpPost]
        public IActionResult CheckOutDevice(AvailableDevicesModel model)
        {
            var idList = model.Devices.Where(d => d.IsSelected).Select(d => d.Id).ToArray();
            _checkouts.CheckOutItems(model.EmployeeId, idList);

            return RedirectToAction("Details", "Employee", new { id = model.EmployeeId });
        }

        // Забрать устройство у сотрудника
        public IActionResult CheckInDevice(int deviceId, int employeeid)
        {
            _checkouts.CheckInItem(deviceId);

            return RedirectToAction("Details", new { id = employeeid });
        }

        // Детальная информация о сотруднике
        public IActionResult Details(int id)
        {
            var employee = _employees.GetById(id);

            var checkouts = employee.Checkouts;

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
    }
}
