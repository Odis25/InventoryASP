using System;

namespace InventoryASP.Models.Device
{
    public class HistoryModel
    {
        public int Id { get; set; }        

        public DateTime Since { get; set; }
        public DateTime? Until { get; set; }

        public Holder Holder { get; set; }
    }
}
