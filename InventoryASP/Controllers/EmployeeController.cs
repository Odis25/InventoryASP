using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Checkouts;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryASP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employees;
        private readonly ICheckout _checkouts;
        private readonly IDepartment _departments;
        private readonly IDevice _devices;

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
        public IActionResult Create(NewEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    Patronymic = model.Patronymic,
                    Position = model.Position,
                    Department = model.Department
                };
                _employees.Add(employee);
            }

            ViewBag.Departments = _departments.GetDepartments();
            ViewBag.Positions = _departments.GetPositions();

            return PartialView("Create", model);
        }

        // Форма изменения данных сотрудника
        public IActionResult Update(int id)
        {
            var employee = _employees.GetById(id);
            var model = new NewEmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Department = employee.Department,
                Position = employee.Position
            };

            ViewBag.Departments = _departments.GetDepartments();
            ViewBag.Positions = _departments.GetPositions();

            return PartialView(model);
        }

        // Изменить данные сотрудника
        [HttpPost]
        public IActionResult Update(NewEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = model.Id,
                    LastName = model.LastName,
                    Name = model.Name,
                    Patronymic = model.Patronymic,
                    Position = model.Position,
                    Department = model.Department
                };
                _employees.Update(employee);
            }

            ViewBag.Departments = _departments.GetDepartments();
            ViewBag.Positions = _departments.GetPositions();

            return PartialView("Update", model);
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

        // Выбрать сотрудника
        public IActionResult SelectEmployee(int deviceId)
        {
            var checkouts = _checkouts.GetCheckout(deviceId);
            if (checkouts != null)
                return null;

            var employees = _employees.GetAll()
                .Select(e => new EmployeeListingModel
                {
                    Id = e.Id,
                    LastName = e.LastName,
                    Patronymic = e.Patronymic,
                    Name = e.Name,
                    Department = e.Department.Name,
                    Position = e.Position.Name
                }).ToList();

            var model = new SelectEmployeeModel
            {
                DeviceId = deviceId,
                Employees = employees
            };

            return PartialView(model);
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
