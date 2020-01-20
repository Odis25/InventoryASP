using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryASP.Models.Device
{
    public class NewDeviceModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceModel { get; set; }
        public string Description { get; set; }
    }
}
