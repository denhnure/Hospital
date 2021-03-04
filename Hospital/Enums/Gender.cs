using System.ComponentModel;

namespace Hospital.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
