using InventoryApp.Services.Interfaces;
using InventoryApp.Models.Device;
using InventoryApp.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkouts;
        private readonly IDeviceService _deviceService;

        public string ReturnUrl { get; set; }

        public CheckoutController(ICheckoutService checkouts, IDeviceService deviceService)
        {
            _checkouts = checkouts;
            _deviceService = deviceService;
        }

        // Добавить устройство сотруднику
        [HttpPost]
        public async Task<IActionResult> CheckOutDevicesAsync(SelectDevicesModel model)
        {
            ReturnUrl = HttpContext.Request.Headers["Referer"];

            var devicesId = model.Devices.Where(d => d.IsSelected).Select(d=> d.Id).ToArray();

            await _checkouts.CheckOutDevices(model.EmployeeId, devicesId);

            return Redirect(ReturnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOutDeviceAsync(SelectEmployeeModel model)
        {
            ReturnUrl = HttpContext.Request.Headers["Referer"];

            await _checkouts.CheckOutDevices(model.SelectedEmployeeId, model.DeviceId);

            return Redirect(ReturnUrl);
        }

        // Забрать устройство у сотрудника
        public IActionResult CheckInDevice(int id)
        {
            ViewBag.Id = id;

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CheckInAsync(int id)
        {
            ReturnUrl = HttpContext.Request.Headers["Referer"];

            await _checkouts.CheckInDevice(id);

            return Redirect(ReturnUrl);
        }
    }
}
