using InventoryAppData.Models;

namespace InventoryASP.Models.Employee
{
    public class NewEmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public Department Department { get; set; }
        public Position Position { get; set; }
    }
}
