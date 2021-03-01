using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using Hospital.Models;
using Hospital.Repositories;

using LocalizationResources = Hospital.Properties.Resources;

namespace Hospital
{
    public partial class App : Application
    {
        private const string CULTURE_NAME = "uk-UA";

        public App()
        {
            Repository.Instance = new SqLiteRepository();
            //AddPatientRecords(100);
            SetCulture();
        }

        //TODO: for load & performance tests
        private static void AddPatientRecords(int amount)
        {
            Repository.Instance.GetPatientRecords();

            for (int index = 1; index <= amount; ++index)
            {
                Repository.Instance.AddPatientRecord(new PatientRecord
                {
                    PatientName = index.ToString(),
                    DoctorName = index.ToString(),
                    FinancialData = new PatientRecordFinancialData
                    {
                        DoctorAmount = index,
                        HospitalAmount = index,
                        Amount = index
                    },
                    VisitDate = DateTime.Today
                });
            }
        }

        private void SetCulture()
        {
            CultureInfo cultureInfo = new CultureInfo(CULTURE_NAME);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            FrameworkElement.LanguageProperty.OverrideMetadata
            (
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag))
            );
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            EventManager.RegisterClassHandler(typeof(DatePicker), FrameworkElement.LoadedEvent, new RoutedEventHandler(DatePicker_Loaded));
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            var datePickerTextBox = GetChildOfType<DatePickerTextBox>(datePicker);
            var datePickerWatermark = datePickerTextBox?.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
            
            if (datePickerWatermark != null)
            {
                datePickerWatermark.Content = LocalizationResources.SelectDate;
            }
        }

        private static T GetChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            { 
                return null;
            }

            for (int index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyObject); index++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, index);
                var result = (child as T) ?? GetChildOfType<T>(child);
                
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
