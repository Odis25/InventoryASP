using InventoryApp.Services.Interfaces;
using InventoryApp.Services.Models;
using InventoryApp.Models.Device;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _devices;

        public DeviceController(IDeviceService devices)
        {
            _devices = devices;
        }

        public async Task<IActionResult> IndexAsync(string sortOrder, string searchPattern)
        {

            var devices = await _devices.GetDevicesAsync(searchPattern);

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SearchPattern"] = string.IsNullOrEmpty(searchPattern) ? "" : searchPattern;
            ViewData["TypeSortParam"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["SnSortParam"] = sortOrder == "sn" ? "sn_desc" : "sn";
            ViewData["YearSortParam"] = sortOrder == "year" ? "year_desc" : "year";
            ViewData["ModelSortParam"] = sortOrder == "model" ? "model_desc" : "model";
            ViewData["ManufacturerSortParam"] = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";

            devices = sortOrder switch
            {
                "name_desc" => devices.OrderByDescending(s => s.DeviceName).ToHashSet(),
                "type" => devices.OrderBy(s => s.DeviceType).ToHashSet(),
                "type_desc" => devices.OrderByDescending(s => s.DeviceType).ToHashSet(),
                "sn" => devices.OrderBy(s => s.SerialNumber).ToHashSet(),
                "sn_desc" => devices.OrderByDescending(s => s.SerialNumber).ToHashSet(),
                "year" => devices.OrderBy(s => s.Year).ToHashSet(),
                "year_desc" => devices.OrderByDescending(s => s.Year).ToHashSet(),
                "model" => devices.OrderBy(s => s.DeviceModel).ToHashSet(),
                "model_desc" => devices.OrderByDescending(s => s.DeviceModel).ToHashSet(),
                "manufacturer" => devices.OrderBy(s => s.DeviceManufacturer).ToHashSet(),
                "manufacturer_desc" => devices.OrderByDescending(s => s.DeviceManufacturer).ToHashSet(),
                _ => devices.OrderBy(s => s.DeviceName).ToHashSet()
            };

            return View(new DeviceIndexModel { Devices = devices });
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            var device = await _devices.GetDeviceByIdAsync(id);

            return View(new DeviceDetailModel { Device = device });
        }

        public IActionResult Create()
        {
            var model = new CreateOrUpdateDeviceModel { Device = new DeviceDto() };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateOrUpdateDeviceModel model)
        {
            if (ModelState.IsValid)
            {
                await _devices.CreateDeviceAsync(model.Device);
            }

            return PartialView("Create", model);
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var device = await _devices.GetDeviceByIdAsync(id);

            var model = new CreateOrUpdateDeviceModel
            {
                Device = device
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(CreateOrUpdateDeviceModel model)
        {
            if (ModelState.IsValid)
            {
                await _devices.UpdateDeviceAsync(model.Device);
            }

            return PartialView("Update", model);
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _devices.DeleteDeviceAsync(id);

            return RedirectToAction("Index", "Device");
        }

        public async Task<IActionResult> SelectDevicesAsync(int employeeId, string sortOrder, string searchPattern)
        {
            var devices = await _devices.GetDevicesAsync(searchPattern, true);

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SearchPattern"] = string.IsNullOrEmpty(searchPattern) ? "" : searchPattern;
            ViewData["TypeSortParam"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["SnSortParam"] = sortOrder == "sn" ? "sn_desc" : "sn";
            ViewData["YearSortParam"] = sortOrder == "year" ? "year_desc" : "year";
            ViewData["ModelSortParam"] = sortOrder == "model" ? "model_desc" : "model";
            ViewData["ManufacturerSortParam"] = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";

            devices = sortOrder switch
            {
                "name_desc" => devices.OrderByDescending(s => s.DeviceName).ToHashSet(),
                "type" => devices.OrderBy(s => s.DeviceType).ToHashSet(),
                "type_desc" => devices.OrderByDescending(s => s.DeviceType).ToHashSet(),
                "sn" => devices.OrderBy(s => s.SerialNumber).ToHashSet(),
                "sn_desc" => devices.OrderByDescending(s => s.SerialNumber).ToHashSet(),
                "year" => devices.OrderBy(s => s.Year).ToHashSet(),
                "year_desc" => devices.OrderByDescending(s => s.Year).ToHashSet(),
                "model" => devices.OrderBy(s => s.DeviceModel).ToHashSet(),
                "model_desc" => devices.OrderByDescending(s => s.DeviceModel).ToHashSet(),
                "manufacturer" => devices.OrderBy(s => s.DeviceManufacturer).ToHashSet(),
                "manufacturer_desc" => devices.OrderByDescending(s => s.DeviceManufacturer).ToHashSet(),
                _ => devices.OrderBy(s => s.DeviceName).ToHashSet()
            };

            var model = new SelectDevicesModel
            {
                Devices = devices.ToArray(),
                EmployeeId = employeeId
            };

            return PartialView(model);
        }
    }
}
