using InventoryApp.Services.Models;
using System.Collections.Generic;

namespace InventoryApp.Services.Interfaces
{
    public interface IDepartmentService
    {
        ICollection<DepartmentDto> Departments { get; }
        ICollection<PositionDto> Positions { get; }
    }
}
