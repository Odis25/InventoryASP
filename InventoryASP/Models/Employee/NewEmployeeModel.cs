using InventoryAppData.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryASP.Models.Employee
{
    public class NewEmployeeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Это поле обязательно для заполнения.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string Patronymic { get; set; }

        public Department Department { get; set; }
        public Position Position { get; set; }
    }
}
