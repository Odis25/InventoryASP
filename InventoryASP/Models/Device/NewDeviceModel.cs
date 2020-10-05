using System.ComponentModel.DataAnnotations;

namespace InventoryASP.Models.Device
{
    public class NewDeviceModel
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceModel { get; set; }
        public string Description { get; set; }
    }
}
