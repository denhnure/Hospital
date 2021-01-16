using System;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Helpers;
using Hospital.Properties;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class DateRangeReportViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int? patientCount;
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

        public double? DoctorAmount => Amount * Constants.DOCTOR_AMOUNT_FACTOR;

        public double? HospitalAmount => Amount * Constants.HOSPITAL_AMOUNT_FACTOR;

        public double? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoctorAmount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HospitalAmount)));
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
                ValidationText = string.Format(Resources.ValidationErrorTemplate, "пациентов не было в клинике в указанный период");
                return;
            }

            PatientCount = patientCount;
            Amount = Repository.Instance.GetAmount(FromDate, ToDate);
        }
    }
}
