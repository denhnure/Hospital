using System;
using Hospital.Properties;

namespace Hospital.Helpers
{
    public static class DateRangeValidator
    {
        public static string Validate(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate > DateTime.Today)
            {
                return string.Format(Resources.ValidationErrorTemplate, "начальная дата не может быть позже сегодняшней даты");
            }

            if (toDate > DateTime.Today)
            {
                return string.Format(Resources.ValidationErrorTemplate, "конечная дата не может быть позже сегодняшней даты");
            }

            if (fromDate > toDate)
            {
                return string.Format(Resources.ValidationErrorTemplate, "неверный диапазон дат. Начальная дата не может быть позже конечной");
            }

            return null;
        }
    }
}
