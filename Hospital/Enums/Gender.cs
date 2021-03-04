using System.ComponentModel;

namespace Hospital.Enums
{
    [TypeConverter(typeof(EnumToLocalisedStringConverter))]
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
