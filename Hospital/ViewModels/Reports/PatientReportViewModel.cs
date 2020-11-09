﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Helpers;
using Hospital.Properties;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class PatientReportViewModel : IPageViewModel, INotifyPropertyChanged
    {
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

        public double? DoctorAmount => Amount * Constants.DOCTOR_AMOUNT;

        public double? HospitalAmount => Amount * Constants.HOSPITAL_AMOUNT;

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
                ValidationText = string.Format(Resources.ValidationErrorTemplate, "введите имя пациента");
                return;
            }

            if (!Repository.Instance.DoesPatientExist(PatientName))
            {
                ValidationText = string.Format(Resources.ValidationErrorTemplate, "данного пациента в системе не обнаружено");
                return;
            }

            string dateRangeValidation = DateRangeValidator.Validate(FromDate, ToDate);

            if (!string.IsNullOrEmpty(dateRangeValidation))
            {
                ValidationText = dateRangeValidation;
                return;
            }

            double? amount = Repository.Instance.GetPatientAmount(PatientName, FromDate, ToDate);

            if(amount == null)
            {
                ValidationText = string.Format(Resources.ValidationErrorTemplate, "данный пациент не был в клинике в указанный период");
                return;
            }

            Amount = amount;
        }
    }
}
