using InventoryAppServices.Models;
using System.Collections.Generic;

namespace InventoryAppServices.Interfaces
{
    public interface IDepartmentService
    {
        ICollection<DepartmentDto> Departments { get; }
        ICollection<PositionDto> Positions { get; }
    }
}
