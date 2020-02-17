using InventoryASP.Models.Employee;
using System;

namespace InventoryASP.Models.Checkouts
{
    public class HistoryModel
    {
        public int Id { get; set; }        

        public string Since { get; set; }
        public string Until { get; set; }

        public EmployeeListingModel Holder { get; set; }
    }
}
