using InventoryAppData;
using InventoryAppData.Models;
using InventoryASP.Models.Device;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Holder = new Holder
                {
                    HolderId = _checkouts.GetCheckoutHolderId(device.Id),
                    HolderFullName = _checkouts.GetCheckoutHolderFullName(device.Id)
                }
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

            var holder = new Holder
            {
                HolderId = _checkouts.GetCheckoutHolderId(id),
                HolderFullName = _checkouts.GetCheckoutHolderFullName(id)
            };

            var history = _checkouts.GetCheckoutHistory(id)
                .Select(h => new HistoryModel
                {
                    Id = h.Id,
                    Since = h.CheckedOut,
                    Until = h.CheckedIn,
                    Holder = new Holder
                    {
                        HolderId = h.Employee.Id,
                        HolderFullName = _checkouts.GetCheckoutHolderFullName(h.Device.Id)
                    }
                });

            var model = new DeviceDetailModel
            {
                DeviceId = device.Id,
                DeviceName = device.Name,
                DeviceType = device.Type,
                DeviceModel = device.DeviceModel,
                DeviceSerialNumber = device.SerialNumber,
                DeviceManufacturer = device.Manufacturer,
                DeviceDescription = device.Description,
                Holder = holder,
                CheckoutHistory = history
            };

            return View(model);
        }

        // Форма добавления нового устройства
        public IActionResult NewDevice()
        {
            var model = new NewDeviceModel();

            return PartialView(model);
        }

        // Добавляем новое устройство
        [HttpPost]
        public IActionResult AddNewDevice(NewDeviceModel model)
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

        // Удалить устройство
        public IActionResult DeleteDevice(int id)
        {
            ViewBag.Id = id;
            return PartialView();
        }

        // Удалить устройство
        [HttpPost]
        public IActionResult DeleteDevicePost(int id)
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
            });

            var model = new AvailableDevicesModel
            {
                Devices = listingResult,
                EmployeeId = employeeId
            };

            return PartialView(model);
        }
    }
}
