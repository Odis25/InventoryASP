using InventoryAppServices.Models;
using System.Collections.Generic;

namespace InventoryASP.Models.Device
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
