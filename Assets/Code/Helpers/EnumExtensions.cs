using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string ToDescriptionString(this Enum val)
    {
        FieldInfo info = val.GetType().GetField(val.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }

        return val.ToString();
    }
}
