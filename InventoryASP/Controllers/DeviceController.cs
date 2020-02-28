using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Checkouts;
using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace InventoryASP.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDevice _devices;
        private readonly ICheckout _checkouts;

        public DeviceController(IDevice devices, ICheckout checkouts)
        {
            _devices = devices;
            _checkouts = checkouts;
        }

        // Список всех устройств
        public IActionResult Index()
        {
            var devices = _devices.GetAll();

            var listingResult = devices.Select(device => new DeviceListingModel
            {
                Id = device.Id,
                DeviceType = device.Type,
                DeviceName = device.Name,
                DeviceModel = device.DeviceModel,
                DeviceManufacturer = device.Manufacturer,
                SerialNumber = device.SerialNumber,
                CurrentHolder = GetDeviceHolder(device)
            });

            var model = new DeviceIndexModel
            {
                Devices = listingResult
            };

            return View(model);
        }

        // Детальная информация об устройстве
        public IActionResult Details(int id)
        {
            var device = _devices.GetById(id);

            var holder = GetDeviceHolder(device);

            var history = _checkouts.GetCheckoutHistory(id)
                .Select(h => new HistoryModel
                {
                    Id = h.Id,
                    Since = h.CheckedOut.ToString(),
                    Until = h.CheckedIn.ToString(),
                    Holder = GetDeviceHolder(h)
                }).ToList();

            var model = new DeviceDetailModel
            {
                DeviceId = device.Id,
                DeviceName = device.Name,
                DeviceType = device.Type,
                DeviceModel = device.DeviceModel,
                DeviceSerialNumber = device.SerialNumber,
                DeviceManufacturer = device.Manufacturer,
                DeviceDescription = device.Description,
                CurrentHolder = holder,
                CheckoutHistory = history
            };

            return View(model);
        }

        // Форма добавления нового оборудования
        public IActionResult Create()
        {
            var model = new NewDeviceModel();

            return PartialView(model);
        }

        // Добавляем новое оборудование
        [HttpPost]
        public IActionResult AddDevice(NewDeviceModel model)
        {
            var device = new Device
            {
                Name = model.Name,
                Type = model.Type,
                SerialNumber = model.SerialNumber,
                Manufacturer = model.Manufacturer,
                DeviceModel = model.DeviceModel,
                Description = model.Description
            };

            _devices.Add(device);

            return RedirectToAction("Index", "Device");
        }

        // Форма изменения данных оборудования
        public IActionResult Update(int id)
        {
            var device = _devices.GetById(id);
            var model = new NewDeviceModel
            {
                Id = device.Id,
                Name = device.Name,
                DeviceModel = device.DeviceModel,
                Manufacturer = device.Manufacturer,
                SerialNumber = device.SerialNumber,
                Type = device.Type,
                Description = device.Description
            };

            return PartialView(model);
        }

        // Изменить данные оборудования
        [HttpPost]
        public IActionResult ModifyDevice(NewDeviceModel model)
        {
            var device = new Device
            {
                Id = model.Id,
                Name = model.Name,
                DeviceModel = model.DeviceModel,
                Manufacturer = model.Manufacturer,
                SerialNumber = model.SerialNumber,
                Type = model.Type,
                Description = model.Description
            };

            _devices.Update(device);

            return RedirectToAction("Index");
        }

        // Удалить оборудование
        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            return PartialView();
        }

        // Удалить оборудование
        [HttpPost]
        public IActionResult DeleteDevice(int id)
        {
            _devices.Delete(id);

            return RedirectToAction("Index", "Device");
        }

        // Получаем список свободного оборудования
        public IActionResult GetAvailableDevices(int employeeId)
        {
            var devices = _devices.GetAvailableDevices();

            var listingResult = devices.Select(device => new DeviceListingModel
            {
                Id = device.Id,
                DeviceType = device.Type,
                DeviceName = device.Name,
                DeviceModel = device.DeviceModel,
                DeviceManufacturer = device.Manufacturer,
                SerialNumber = device.SerialNumber
            }).ToList();

            var model = new AvailableDevicesModel
            {
                Devices = listingResult,
                EmployeeId = employeeId
            };

            return PartialView(model);
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
        private EmployeeListingModel GetDeviceHolder(CheckoutHistory history)
        {
            var employee = history.Employee;

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
