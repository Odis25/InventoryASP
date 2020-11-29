using InventoryAppData;
using InventoryAppServices.Interfaces;
using InventoryAppServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAppServices
{
    public class DepartmentService : IDepartmentService
    {
        public DepartmentService(AppDbContext context)
        {
            Departments = context.Departments.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name }).ToHashSet();
            Positions = context.Positions.Select(p => new PositionDto { Id = p.Id, Name = p.Name }).ToHashSet();
        }

        public ICollection<DepartmentDto> Departments { get; private set; }
        public ICollection<PositionDto> Positions { get; private set; }
    }
}
