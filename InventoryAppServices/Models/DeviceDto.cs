using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryAppServices.Models
{
    public class DeviceDto
    {
        public int Id { get; set; }

        public string DeviceType { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string DeviceName { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceManufacturer { get; set; }

        public string SerialNumber { get; set; }

        public string Description { get; set; }

        public bool IsSelected { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }

        public CheckoutDto Checkout { get; set; }
        public ICollection<CheckoutHistoryDto> CheckoutsHistory { get; set; }
    }
}
