using InventoryASP.Models.Employee;

namespace InventoryASP.Models.Device
{
    public class DeviceListingModel
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
        public string SerialNumber { get; set; }

        public bool IsSelected { get; set; }

        public EmployeeListingModel CurrentHolder { get; set; }
    }
}
