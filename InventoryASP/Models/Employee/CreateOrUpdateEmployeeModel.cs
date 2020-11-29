using InventoryAppServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryASP.Models.Employee
{
    public class CreateOrUpdateEmployeeModel
    {
        public EmployeeDto Employee { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Positions { get; set; }

    }
}
