using InventoryAppData;
using InventoryASP.Models.Device;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

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
                HolderName = GetHolderFullName(device.Id)
            });

            var model = new DeviceIndexModel
            {
                Devices = listingResult
            };

            return View(model);
        }

        // Формируем фамилию и инициалы
        private string GetHolderFullName(int id)
        {
            var holder = _deviceService.GetCurrentHolder(id);
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
    }
}
