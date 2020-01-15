using System.Collections.Generic;

namespace InventoryASP.Models.Catalog
{
    public class DeviceIndexModel
    {
        public IEnumerable<DeviceIndexListingModel> Devices { get; set; }
    }
}
