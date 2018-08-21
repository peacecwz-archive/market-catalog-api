using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

public static class EnumExtensions
{
    public static string GetDescriptionString(this Enum val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
