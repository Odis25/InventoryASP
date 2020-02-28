using System.ComponentModel.DataAnnotations;

namespace InventoryASP.Models.Account
{
    public class ManageUserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Текущий пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="Поле обязательно для заполнения")]
        [Display(Name = "Подтверждение нового пароля")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Введенные пароли не совпадают")]
        public string ConfirmNewPassword { get; set; }
    }
}
