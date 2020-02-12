using System;

namespace InventoryASP.Models.Device
{
    public class DeviceHistoryModel
    {
        public int Id { get; set; }
        public int HolderId { get; set; }

        public DateTime Since { get; set; }
        public DateTime? Until { get; set; }

        public string HolderFullName { get; set; }
    }
}
