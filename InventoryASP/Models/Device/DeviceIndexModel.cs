using System.Collections.Generic;

namespace InventoryASP.Models.Device
{
    public class DeviceIndexModel
    {
        public IEnumerable<DeviceListingModel> Devices { get; set; }
    }
}
