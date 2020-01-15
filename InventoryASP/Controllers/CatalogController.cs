using InventoryAppData;
using InventoryASP.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryASP.Controllers
{
    public class CatalogController : Controller
    {
        private IDevice _device;

        public CatalogController(IDevice device)
        {
            _device = device;
        }

        public IActionResult Index()
        {
            var devices = _device.GetAll().ToList();

            var listingResult = devices.Select(device => new DeviceIndexListingModel
            {
                Id = device.Id,
                Type = device.Type,
                Name = device.Name,
                DeviceModel = device.DeviceModel,
                SerialNumber = device.SerialNumber,
                HolderName = _device.GetCurrentHolder(device.Id).LastName
                + " "
                + _device.GetCurrentHolder(device.Id).Name.First()
                + "."
                + _device.GetCurrentHolder(device.Id).Patronymic.First()
                + "."
            });

            var model = new DeviceIndexModel
            {
                Devices = listingResult
            };

            return View(model);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}