namespace InventoryAppData.Models
{
    public class Device
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceModel { get; set; }
        public string Description { get; set; }
        
        public string Status { get; set; }
    }
}
