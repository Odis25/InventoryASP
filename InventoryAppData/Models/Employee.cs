using System.Collections.Generic;

namespace InventoryAppData.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public Department Department { get; set; }
        public Position Position { get; set; }

        public virtual IEnumerable<Checkout> Checkouts { get; set; }
    }
}
