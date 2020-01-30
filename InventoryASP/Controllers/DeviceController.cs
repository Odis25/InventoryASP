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
                HolderId = GetHolderId(device.Id),
                HolderName = GetHolderFullName(device.Id)
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

            var model = new DeviceDetailsModel
            {
                Id = device.Id,
                DeviceName = device.Name,
                DeviceType = device.Type,
                DeviceModel = device.DeviceModel,
                DeviceSerialNumber = device.SerialNumber,
                DeviceManufacturer = device.Manufacturer,
                DeviceDescription = device.Description,
                DeviceHistory = _deviceService.GetDeviceHistory(id),
                DeviceCurrentHolder = _deviceService.GetCurrentHolder(id)
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
        [HttpPost]
        public IActionResult DeleteDevice(DeviceIndexModel model)
        {
            var idList = model.Devices.Where(d => d.IsSelected).Select(device => device.Id).ToArray();

            _deviceService.Delete(idList);
            return RedirectToAction("Index","Device");
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
                HolderName = GetHolderFullName(device.Id)
            }).ToList();

            var model = new AvalibleDevicesModel
            {
                Devices = listingResult,
                EmployeeId = employeeId
            };

            return PartialView(model);
        }

        // Получаем Id сотрудника
        private int GetHolderId(int id)
        {
            var holder = _deviceService.GetCurrentHolder(id);

            var holderId = holder?.Id ?? 0;

            return holderId;
        }
        // Формируем фамилию и инициалы
        private string GetHolderFullName(int id)
        {
            var holder = _deviceService.GetCurrentHolder(id);
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
                Status = "Avalible"
            };

            return device;
        }
    }
}
