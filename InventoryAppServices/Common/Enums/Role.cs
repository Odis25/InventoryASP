using System.ComponentModel.DataAnnotations;

namespace InventoryAppServices.Common.Enums
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
