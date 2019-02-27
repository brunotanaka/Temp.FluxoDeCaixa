using System;
using System.ComponentModel;
using System.Linq;

namespace FluxoDeCaixa.Application.Util
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var customAttributes = type.GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return customAttributes.Any() ? ((DescriptionAttribute)customAttributes.FirstOrDefault()).Description : value.ToString();
        }
    }
}
