using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InventoryApp.Services.Models
{
    public class EmployeeDto
    {
        public EmployeeDto()
        {
            Checkouts = new HashSet<CheckoutDto>();
            CheckoutHistory = new HashSet<CheckoutHistoryDto>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        public string Patronymic { get; set; }

        public string ImageUrl { get; set; }

        public DepartmentDto Department { get; set; }

        public PositionDto Position { get; set; }

        public bool IsSelected { get; set; }

        public ICollection<CheckoutDto> Checkouts { get; set; }

        public ICollection<CheckoutHistoryDto> CheckoutHistory { get; set; }

        public string FullName => $"{LastName} {Name?.First()}.{Patronymic?.First()}.";        
    }
}
