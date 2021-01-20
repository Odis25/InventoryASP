using InventoryApp.Services.Models;

namespace InventoryApp.Models.Device
{
    public class SelectDevicesModel
    {
        public DeviceDto[] Devices { get; set; }
        public int EmployeeId { get; set; }
    }
}
