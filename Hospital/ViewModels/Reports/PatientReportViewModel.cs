using System;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class PatientReportViewModel : IPageViewModel, INotifyPropertyChanged
    {
        private const string VALIDATION_ERROR_TEMPLATE = "Ошибка: {0}!";

        public event PropertyChangedEventHandler PropertyChanged;
        private double? amount;
        private string validationText;

        public PatientReportViewModel()
        {
            CreatePatientReportCommand = new RelayCommand(CreatePatientReport);
        }

        public string PatientName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public double? DoctorAmount => Amount * 0.6;

        public double? HospitalAmount => Amount * 0.4;

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

        public ICommand CreatePatientReportCommand { get; private set; }

        private void CreatePatientReport(object obj)
        {
            Amount = null;
            ValidationText = null;

            if (string.IsNullOrEmpty(PatientName))
            {
                ValidationText = string.Format(VALIDATION_ERROR_TEMPLATE, "введите имя пациента");
                return;
            }

            if (!Repository.Instance.DoesPatientExist(PatientName))
            {
                ValidationText = string.Format(VALIDATION_ERROR_TEMPLATE, "данного пациента в системе не обнаружено");
                return;
            }

            if (FromDate > ToDate)
            {
                ValidationText = string.Format(VALIDATION_ERROR_TEMPLATE, "неверный диапазон дат. Начальная дата не может быть позже конечной");
                return;
            }

            double? amount = Repository.Instance.GetAmount(PatientName, FromDate, ToDate);

            if(amount == null)
            {
                ValidationText = string.Format(VALIDATION_ERROR_TEMPLATE, "данный пациент не был в клинике в указанный период");
                return;
            }

            Amount = amount;
        }
    }
}
