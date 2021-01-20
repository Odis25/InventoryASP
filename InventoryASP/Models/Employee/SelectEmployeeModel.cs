using InventoryApp.Services.Models;

namespace InventoryApp.Models.Employee
{
    public class SelectEmployeeModel
    {
        public EmployeeDto[] Employees { get; set; }
        public int SelectedEmployeeId { get; set; }
        public int DeviceId { get; set; }

    }
}
