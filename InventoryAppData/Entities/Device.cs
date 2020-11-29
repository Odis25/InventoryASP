using InventoryAppData.Common;

namespace InventoryAppData.Entities
{
    public class Device: AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DeviceModel { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }
    }
}
