using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Helpers;
using Hospital.Models;
using Hospital.Properties;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class PatientReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double? doctorAmount;
        private double? hospitalAmount;
        private double? amount;
        private string validationText;
        private ObservableCollection<PatientRecord> patientRecords;

        public PatientReportViewModel()
        {
            CreatePatientReportCommand = new RelayCommand(CreatePatientReport, _ => !string.IsNullOrEmpty(PatientName));
        }

        public string PatientName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

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

        public ObservableCollection<PatientRecord> PatientRecords
        {
            get { return patientRecords; }
            private set
            {
                patientRecords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PatientRecords)));
            }
        }

        public ICommand CreatePatientReportCommand { get; private set; }

        private void CreatePatientReport(object obj)
        {
            DoctorAmount = null;
            HospitalAmount = null;
            Amount = null;
            PatientRecords = null;
            ValidationText = null;

            if (!Repository.Instance.DoesPatientExist(PatientName))
            {
                ValidationText = string.Format(Resources.ValidationErrorTemplate, Resources.PatientNotFound);
                return;
            }

            string dateRangeValidation = DateRangeValidator.Validate(FromDate, ToDate);

            if (!string.IsNullOrEmpty(dateRangeValidation))
            {
                ValidationText = dateRangeValidation;
                return;
            }

            PatientRecordFinancialData aggregatedPatientFinancialData = Repository.Instance.GetAggregatedPatientFinancialData(PatientName, FromDate, ToDate);

            if(aggregatedPatientFinancialData == null)
            {
                ValidationText = string.Format(Resources.ValidationErrorTemplate, Resources.PatientWasNotAtClinicDuringSpecifiedPeriod);
                return;
            }

            DoctorAmount = aggregatedPatientFinancialData.DoctorAmount;
            HospitalAmount = aggregatedPatientFinancialData.HospitalAmount;
            Amount = aggregatedPatientFinancialData.Amount;
            PatientRecords = Repository.Instance.GetSpecificPatientRecords(PatientName, FromDate, ToDate);
        }
    }
}
