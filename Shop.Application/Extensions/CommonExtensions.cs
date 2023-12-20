using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Shop.Application.Extensions
{
    public static class CommonExtensions
    {
        public static string GetEnumName(this Enum myEnum)
        {
            var enumDisplayName = myEnum.GetType()
                .GetMember(myEnum.ToString())
                .FirstOrDefault();
            return enumDisplayName != null ? (enumDisplayName.GetCustomAttribute<DisplayAttribute>().GetName() ?? "") : "";
        }
    }
}
