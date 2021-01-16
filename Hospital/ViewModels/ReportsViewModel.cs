using System.Collections.Generic;
using Hospital.Properties;
using Hospital.ViewModels.Reports;

namespace Hospital.ViewModels
{
    public class ReportsViewModel : IPageViewModel
    {
        public List<object> Tabs { get; private set; }

        public ReportsViewModel()
        {
            Tabs = new List<object>
            {
                new { Header = Resources.PatientReport, Content = new PatientReportViewModel() },
                new { Header = Resources.DateRangeReport, Content = new DateRangeReportViewModel() },
                new { Header = Resources.SpecificDateReport, Content = new SpecificDateReportViewModel() }
            };
        }
    }
}
