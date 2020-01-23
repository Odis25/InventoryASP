using InventoryAppData;
using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppServices
{
    public class PositionService : IPosition
    {
        private readonly InventoryContext _context;

        public PositionService(InventoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Position> GetAll()
        {
            return _context.Positions;
        }

        public Position GetPosition(int id)
        {
            return _context.Positions.Find(id);
        }
    }
}
