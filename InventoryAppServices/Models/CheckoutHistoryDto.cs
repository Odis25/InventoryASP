using System;

namespace InventoryAppServices.Models
{
    public class CheckoutHistoryDto
    {
        public int Id { get; set; }

        public DateTime Since { get; set; }
        public DateTime? Until { get; set; }

        public DeviceDto Device { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
