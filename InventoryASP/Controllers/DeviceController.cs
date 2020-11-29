using InventoryAppServices.Interfaces;
using InventoryAppServices.Models;
using InventoryASP.Models.Device;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryASP.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _devices;

        public DeviceController(IDeviceService devices)
        {
            _devices = devices;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var devices = await _devices.GetDevicesAsync();

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

        public async Task<IActionResult> SelectDevicesAsync(int employeeId)
        {
            var devices = await _devices.GetDevicesAsync(true);

            var model = new SelectDevicesModel
            {
                Devices = devices.ToArray(),
                EmployeeId = employeeId
            };

            return PartialView(model);
        }
    }
}
