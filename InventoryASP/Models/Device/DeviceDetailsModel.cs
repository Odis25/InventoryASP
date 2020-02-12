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

        public string HolderFullName { get; set; }
        public int? HolderId { get; set; }

        public IEnumerable<DeviceHistoryModel> DeviceHistory { get; set; }
    }
}
