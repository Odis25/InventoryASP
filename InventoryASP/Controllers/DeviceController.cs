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
        private readonly IDevice _deviceService;

        public DeviceController(IDevice deviceService)
        {
            _deviceService = deviceService;
        }

        public IActionResult Index()
        {
            var devices = _deviceService.GetAll();

            var listingResult = devices.Select(device => new DeviceListingModel
            {
                Id = device.Id,
                DeviceType = device.Type,
                DeviceName = device.Name,
                DeviceModel = device.DeviceModel,
                DeviceManufacturer = device.Manufacturer,
                SerialNumber = device.SerialNumber,
                HolderFullName = GetHolderFullName(device.Id),
                HolderId = _deviceService.GetCurrentHolder(device.Id)?.Id
            }).ToList();

            var model = new DeviceIndexModel
            {
                Devices = listingResult
            };

            return View(model);
        }

        // Детальная информация об устройстве
        public IActionResult Details(int id)
        {
            var device = _deviceService.GetById(id);
            var holder = _deviceService.GetCurrentHolder(id);
            var history = _deviceService.GetDeviceHistory(id).Select(h => new DeviceHistoryModel
            {
                Id = h.Id,
                Since = h.CheckedOut,
                Until = h.CheckedIn,
                HolderId = h.Employee.Id,
                HolderFullName = GetHolderFullName(h.Device.Id)
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
                LatestCheckout = ,
                HolderFullName = holder != null ? GetHolderFullName(device.Id) : "---"
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
            var device = BuildNewDevice(model);

            _deviceService.Add(device);
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
            _deviceService.Delete(id);

            return RedirectToAction("Index", "Device");
        }

        // Получаем список свободного оборудования
        public IActionResult GetAvalibleDevices(int employeeId)
        {
            var devices = _deviceService.GetAvalibleDevices();

            var listingResult = devices.Select(device => new DeviceListingModel
            {
                Id = device.Id,
                DeviceType = device.Type,
                DeviceName = device.Name,
                DeviceModel = device.DeviceModel,
                DeviceManufacturer = device.Manufacturer,
                SerialNumber = device.SerialNumber,
                HolderFullName = GetHolderFullName(device.Id)
            }).ToList();

            var model = new AvalibleDevicesModel
            {
                Devices = listingResult,
                EmployeeId = employeeId
            };

            return PartialView(model);
        }

        // Формируем фамилию и инициалы
        private string GetHolderFullName(int deviceId)
        {
            var holder = _deviceService.GetCurrentHolder(deviceId);

            if (holder == null)
                return "";

            var holderFullName = new StringBuilder()
                .Append(holder.LastName)
                .Append(" ")
                .Append(holder.Name.First())
                .Append(".")
                .Append(holder.Patronymic.First())
                .Append(".")
                .ToString();

            return holderFullName;
        }
        // Формируем новое устройство
        private Device BuildNewDevice(NewDeviceModel model)
        {
            var device = new Device
            {
                Name = model.Name,
                Type = model.Type,
                SerialNumber = model.SerialNumber,
                Manufacturer = model.Manufacturer,
                DeviceModel = model.DeviceModel,
                Description = model.Description,
                Status = "Available"
            };

            return device;
        }
    }
}
