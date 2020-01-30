using System.Collections.Generic;

namespace InventoryAppData.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Status { get; set; }

        public virtual Department Department { get; set; }
        public virtual Position Position { get; set; }

        public virtual IEnumerable<Checkout> Checkouts { get; set; }
    }
}
