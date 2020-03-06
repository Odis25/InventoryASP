using InventoryAppData;
using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryASP.Controllers
{
    public class CheckoutController: Controller
    {
        private ICheckout _checkouts;

        public CheckoutController(ICheckout checkouts)
        {
            _checkouts = checkouts;
        }

        // Добавить устройство сотруднику
        [HttpPost]
        public IActionResult CheckOutDevice(AvailableDevicesModel model)
        {
            var idList = model.Devices.Where(d => d.IsSelected).Select(d => d.Id).ToArray();
            _checkouts.CheckOutItems(model.EmployeeId, idList);

            return RedirectToAction("Details", "Employee", new { id = model.EmployeeId });
        }

        [HttpPost]
        public IActionResult CheckOutEmployee(int employeeId, int deviceId)
        {
            _checkouts.CheckOutItems(employeeId, deviceId);

            return RedirectToAction("Details", "Device", new { id = deviceId });
        }

        // Забрать устройство у сотрудника
        public IActionResult CheckInDevice(int deviceId, int employeeid)
        {
            _checkouts.CheckInItem(deviceId);

            return RedirectToAction("Details", "Employee", new { id = employeeid });
        }

    }
}
