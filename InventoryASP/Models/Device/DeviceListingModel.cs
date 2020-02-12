namespace InventoryASP.Models.Device
{
    public class DeviceListingModel
    {
        public int Id { get; set; }
        public int? HolderId { get; set; }

        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
        public string SerialNumber { get; set; }      
        public string HolderFullName { get; set; }

        public bool IsSelected { get; set; }
    }
}
