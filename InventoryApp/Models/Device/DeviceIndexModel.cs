using InventoryApp.Services.Models;
using System.Collections.Generic;

namespace InventoryApp.Models.Device
{
    public class DeviceIndexModel
    {
        public DeviceIndexModel()
        {
            Devices = new HashSet<DeviceDto>();
        }

        public ICollection<DeviceDto> Devices { get; set; }
    }
}
