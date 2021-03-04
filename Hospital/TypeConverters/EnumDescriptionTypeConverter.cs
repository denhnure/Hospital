using System;
using System.ComponentModel;
using Hospital.Properties;

namespace Hospital.Enums
{
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value != null
                    ? Resources.ResourceManager.GetString(value.ToString())
                    : string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
