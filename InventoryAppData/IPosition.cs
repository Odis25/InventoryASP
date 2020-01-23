using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IPosition
    {
        IEnumerable<Position> GetAll();
        Position GetPosition(int id);
    }
}
