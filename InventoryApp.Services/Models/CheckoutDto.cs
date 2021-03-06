﻿using System;

namespace InventoryApp.Services.Models
{
    public class CheckoutDto
    {
        public DeviceDto Device { get; set; }
        public EmployeeDto Employee { get; set; }

        public DateTime Since { get; set; }
        public bool IsSelected { get; set; }
    }
}
