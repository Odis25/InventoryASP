using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryASP.Models.Device
{
    public class DeviceDetailsModel
    {
        public int Id { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string DeviceDescription { get; set; }

        public InventoryAppData.Models.Employee DeviceCurrentHolder { get; set; }
        public IEnumerable<CheckoutHistory> DeviceHistory { get; set; }
    }
}
