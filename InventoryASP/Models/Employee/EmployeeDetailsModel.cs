using InventoryAppData.Models;
using InventoryASP.Models.Checkouts;
using System.Collections.Generic;

namespace InventoryASP.Models.Employee
{
    public class EmployeeDetailsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }

        public IEnumerable<CheckoutModel> Checkouts { get; set; }
        public IEnumerable<CheckoutHistory> History { get; set; }
    }
}
