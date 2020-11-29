using InventoryAppServices.Models;

namespace InventoryASP.Models.Device
{
    public class SelectDevicesModel
    {
        public DeviceDto[] Devices { get; set; }
        public int EmployeeId { get; set; }
    }
}
