using Hospital.Properties;

namespace Hospital.ViewModels.Reports
{
    // ВСЕ ПАЦИЕНТЫ ЗА ПЕРИОД С... ПО...
    // С 5 ПО 10 ЧИСЛО, НАПРИМЕР, 20 ПАЦИЕНТОВ И СУММЫ? -> строка
    public class DateRangeReportViewModel : IPageViewModel
    {
        public string Header => Resources.DateRangeReport;
    }
}
