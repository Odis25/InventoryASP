﻿using System.Collections.Generic;

namespace InventoryASP.Models.Device
{
    public class DeviceDetailModel
    {
        public int DeviceId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string DeviceDescription { get; set; }

        public Holder Holder { get; set; }
        public IEnumerable<HistoryModel> CheckoutHistory { get; set; }
    }
}