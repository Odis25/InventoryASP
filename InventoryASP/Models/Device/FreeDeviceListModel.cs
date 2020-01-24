using System.Collections.Generic;

namespace InventoryASP.Models.Device
{
    public class FreeDeviceListModel
    {
        public List<DeviceListingModel> Devices { get; set; }
        public int EmployeeId { get; set; }
    }
}
