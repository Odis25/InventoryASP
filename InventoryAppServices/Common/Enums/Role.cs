using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Services.Common.Enums
{
    public enum Role
    {
        [Display( Name = "Пользователь")]
        User,
        [Display(Name = "Модератор")]
        SuperUser,
        [Display(Name = "Администратор")]
        Admin
    }
}
