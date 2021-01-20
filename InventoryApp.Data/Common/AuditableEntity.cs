using System;

namespace InventoryApp.Data.Common
{
    public abstract class AuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
