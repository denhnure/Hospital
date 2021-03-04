using System;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Helpers;
using Hospital.Models;
using Hospital.Properties;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class DateRangeReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int? patientCount;
        private double? doctorAmount;
        private double? hospitalAmount;
        private double? amount;
        private string validationText;

        public DateRangeReportViewModel()
        {
            CreateDateRangeReportCommand = new RelayCommand(CreateDateRangeReport);
        }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? PatientCount
        {
            get { return patientCount; }
            set
            {
                patientCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PatientCount)));
            }
        }

        public double? DoctorAmount
        {
            get { return doctorAmount; }
            set
            {
                doctorAmount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoctorAmount)));
            }
        }

        public double? HospitalAmount
        {
            get { return hospitalAmount; }
            set
            {
                hospitalAmount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HospitalAmount)));
            }
        }

        public double? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
            }
        }

        public string ValidationText
        {
            get { return validationText; }
            set
            {
                validationText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationText)));
            }
        }

        public ICommand CreateDateRangeReportCommand { get; private set; }

        private void CreateDateRangeReport(object obj)
        {
            PatientCount = null;
            DoctorAmount = null;
            HospitalAmount = null;
            Amount = null;
            ValidationText = null;

            string dateRangeValidation = DateRangeValidator.Validate(FromDate, ToDate);

            if (!string.IsNullOrEmpty(dateRangeValidation))
            {
                ValidationText = dateRangeValidation;
                return;
            }

            int patientCount = Repository.Instance.GetPatientCount(FromDate, ToDate);

            if (patientCount == 0)
            {
                ValidationText = string.Format(Resources.ValidationErrorTemplate, Resources.PatientsWereNotAtClinicDuringSpecifiedPeriod);
                return;
            }

            PatientCount = patientCount;
            PatientRecordFinancialData aggregatedFinancialData = Repository.Instance.GetAggregatedFinancialData(FromDate, ToDate);
            DoctorAmount = aggregatedFinancialData.DoctorAmount;
            HospitalAmount = aggregatedFinancialData.HospitalAmount;
            Amount = aggregatedFinancialData.Amount;
        }
    }
}
