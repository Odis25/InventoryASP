using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryASP.Models.Employee
{
    public class EmployeeListingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string FullName
        {
            get
            {
                var fullName = new StringBuilder()
                .Append(LastName)
                .Append(" ")
                .Append(Name.First())
                .Append(".")
                .Append(Patronymic.First())
                .Append(".")
                .ToString();
                return fullName;
            }
        }

        public bool IsSelected { get; set; }

        public IEnumerable<InventoryAppData.Models.Device> Devices { get; set; }
    }
}
