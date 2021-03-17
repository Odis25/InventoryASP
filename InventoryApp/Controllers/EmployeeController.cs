using InventoryApp.Services.Interfaces;
using InventoryApp.Services.Models;
using InventoryApp.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employees;
        private readonly ICheckoutService _checkouts;
        private readonly IDepartmentService _departments;

        public EmployeeController(
            IEmployeeService employees,
            ICheckoutService checkouts,
            IDepartmentService departments)
        {
            _employees = employees;
            _checkouts = checkouts;
            _departments = departments;
        }

        // Список сотрудников
        public async Task<IActionResult> IndexAsync(string sortOrder)
        {
            var employees = await _employees.GetEmployeesAsync();

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DepartmentSortParam"] = sortOrder == "department" ? "department_desc" : "department";
            ViewData["PositionSortParam"] = sortOrder == "position" ? "position_desc" : "position";

            employees = sortOrder switch
            {
                "name_desc" => employees.OrderByDescending(e=> e.FullName).ToHashSet(),
                "department" => employees.OrderBy(e=> e.Department.Id).ToHashSet(),
                "department_desc" => employees.OrderByDescending(e=> e.Department.Id).ToHashSet(),
                "position" => employees.OrderBy(e=> e.Position.Id).ToHashSet(),
                "position_desc" => employees.OrderByDescending(e=> e.Position.Id).ToHashSet(),
                _ => employees.OrderBy(e=> e.FullName).ToHashSet()
            };

            var model = new EmployeeIndexModel { Employees = employees };

            return View(model);
        }

        // Форма добавления нового сотрудника
        public IActionResult Create()
        {
            var model = new CreateOrUpdateEmployeeModel
            {
                Employee = new EmployeeDto(),
                Departments = new SelectList(_departments.Departments, "Id", "Name"),
                Positions = new SelectList(_departments.Positions, "Id", "Name")
            };

            return PartialView(model);
        }

        // Добавить нового сотрудника
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateOrUpdateEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                await _employees.CreateEmployeeAsync(model.Employee);
            }

            model.Departments = new SelectList(_departments.Departments, "Id", "Name");
            model.Positions = new SelectList(_departments.Positions, "Id", "Name");

            return PartialView("Create", model);
        }

        // Форма изменения данных сотрудника
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var employee = await _employees.GetEmployeeByIdAsync(id);

            var model = new CreateOrUpdateEmployeeModel
            {
                Employee = employee,
                Departments = new SelectList(_departments.Departments, "Id", "Name"),
                Positions = new SelectList(_departments.Positions, "Id", "Name")
            };

            return PartialView(model);
        }

        // Изменить данные сотрудника
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(CreateOrUpdateEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                await _employees.UpdateEmployeeAsync(model.Employee);
            }

            model.Departments = new SelectList(_departments.Departments, "Id", "Name");
            model.Positions = new SelectList(_departments.Positions, "Id", "Name");

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
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _employees.DeleteEmployeeAsync(id);

            return RedirectToAction("Index", "Employee");
        }

        // Детальная информация о сотруднике
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var employee = await _employees.GetEmployeeByIdAsync(id);

            return View(new EmployeeDetailModel { Employee = employee });
        }

        // Выбрать сотрудника
        public async Task<IActionResult> SelectEmployeeAsync(int deviceId)
        {
            var employees = await _employees.GetEmployeesAsync();

            var model = new SelectEmployeeModel
            {
                Employees = employees.ToArray(),
                DeviceId = deviceId
            };

            return PartialView(model);
        }
    }
}
