using InventoryAppData;
using InventoryASP.Models.Device;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryASP.Controllers
{
    public class CheckoutController : Controller
    {
        private ICheckout _checkouts;

        public string ReturnUrl { get; set; }

        public CheckoutController(ICheckout checkouts)
        {
            _checkouts = checkouts;
        }

        // Добавить устройство сотруднику
        [HttpPost]
        public IActionResult CheckOutDevice(AvailableDevicesModel model)
        {
            ReturnUrl = HttpContext.Request.Headers["Referer"];
            var idList = model.Devices.Where(d => d.IsSelected).Select(d => d.Id).ToArray();
            _checkouts.CheckOutItems(model.EmployeeId, idList);

            return Redirect(ReturnUrl);

        }

        [HttpPost]
        public IActionResult CheckOutEmployee(int employeeId, int deviceId)
        {
            ReturnUrl = HttpContext.Request.Headers["Referer"];
            _checkouts.CheckOutItems(employeeId, deviceId);

            return Redirect(ReturnUrl);
        }

        // Забрать устройство у сотрудника
        //public IActionResult CheckInDevice(int deviceId, int employeeid)
        //{
        //    ReturnUrl = HttpContext.Request.Headers["Referer"];
        //    _checkouts.CheckInItem(deviceId);

        //    return Redirect(ReturnUrl);
        //}

        public IActionResult CheckInDevice(int deviceId)
        {
            ViewBag.DeviceId = deviceId;
            return PartialView() ;
        }

        [HttpPost]
        public IActionResult CheckIn(int deviceId)
        {
            ReturnUrl = HttpContext.Request.Headers["Referer"];
            _checkouts.CheckInItem(deviceId);

            return Redirect(ReturnUrl);
        }
    }
}
