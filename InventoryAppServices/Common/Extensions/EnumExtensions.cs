using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace InventoryAppServices.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum context)
        {
            return context.GetType()
                .GetMember(context.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
    }
}
