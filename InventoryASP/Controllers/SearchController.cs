using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Checkouts;
using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using InventoryASP.Models.Search;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace InventoryASP.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearch _search;
        private readonly ICheckout _checkouts;

        public SearchController(ISearch search, ICheckout checkouts)
        {
            _search = search;
            _checkouts = checkouts;
        }

        public IActionResult Find(string searchQuery)
        {
            var employees = _search.SearchEmployee(searchQuery);
            var devices = _search.SearchDevice(searchQuery);

            var model = new SearchResultModel
            {
                Devices = BuildDeviceListingModel(devices),
                Employees = BuildEmployeeListingModel(employees),
                SearchQuery = searchQuery
            };
            return View("SearchResult", model);
        }

        public IActionResult FindEmployee(string searchQuery)
        {
            var employees = _search.SearchEmployee(searchQuery);
            var model = new SearchResultModel
            {
                Employees = BuildEmployeeListingModel(employees),
                SearchQuery = searchQuery
            };

            return View("SearchResult", model);
        }

        public IActionResult FindDevice(string searchQuery)
        {
            var devices = _search.SearchDevice(searchQuery);

            var model = new SearchResultModel
            {
                Devices = BuildDeviceListingModel(devices),
                SearchQuery = searchQuery
            };
            return View("SearchResult", model);
        }

        private IEnumerable<EmployeeListingModel> BuildEmployeeListingModel(IEnumerable<Employee> employees)
        {
            return employees?.Select(e => new EmployeeListingModel
            {
                Id = e.Id,
                Name = e.Name,
                LastName = e.LastName,
                Patronymic = e.Patronymic,
                Position = e.Position.Name,
                Department = e.Department.Name,
                Checkouts = e.Checkouts.Select(c => new CheckoutModel { Checkout = c })
            });
        }

        private IEnumerable<DeviceListingModel> BuildDeviceListingModel(IEnumerable<Device> devices)
        {
            return devices?.Select(d => new DeviceListingModel
            {
                Id = d.Id,
                DeviceName = d.Name,
                DeviceModel = d.DeviceModel,
                DeviceManufacturer = d.Manufacturer,
                DeviceType = d.Type,
                SerialNumber = d.SerialNumber,
                CurrentHolder = GetDeviceHolder(d)
            });
        }

        private EmployeeListingModel GetDeviceHolder(Device device)
        {
            var employee = _checkouts.GetCheckout(device.Id)?.Employee;

            if (employee == null)
                return null;

            return new EmployeeListingModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Department = employee.Department.Name,
                Position = employee.Position.Name
            };
        }
    }
}
