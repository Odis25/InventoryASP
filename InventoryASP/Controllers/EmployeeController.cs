using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Checkouts;
using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IDepartment _departments;

        public EmployeeController(IEmployee employees, ICheckout checkouts, IDepartment departments)
        {
            _employees = employees;
            _checkouts = checkouts;
            _departments = departments;
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
                Checkouts = GetCheckouts(employee)
            });

            var model = new EmployeeIndexModel
            {
                Employees = listingResult
            };

            return View(model);
        }

        // Форма добавления нового сотрудника
        public IActionResult Create()
        {
            ViewBag.Departments = _departments.GetDepartments(); 
            ViewBag.Positions = _departments.GetPositions();

            return PartialView();
        }

        // Добавить нового сотрудника
        [HttpPost]
        public IActionResult AddEmployee(NewEmployeeModel model)
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
        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            return PartialView();
        }

        // Удалить сотрудника
        [HttpPost]
        public IActionResult DeleteEmployee(int id)
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
            var checkouts = GetCheckouts(employee);
            var checkoutsHistory = _employees.GetCheckoutHistory(id);

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

        private IEnumerable<CheckoutModel> GetCheckouts(Employee employee)
        {
            return employee.Checkouts.Select(checkout => new CheckoutModel
            {
                Checkout = checkout
            });
        }
    }
}
