using System;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Helpers;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.ViewModels
{
    public class AddNewPatientRecordViewModel
    {
        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Amount { get; set; }

        public DateTime VisitDate { get; set; }

        public ICommand SaveNewPatientRecordCommand { get; private set; }

        public ICommand CancelNewPatientRecordCommand { get; private set; }

        public AddNewPatientRecordViewModel()
        {
            VisitDate = DateTime.Now;
            SaveNewPatientRecordCommand = new RelayCommand(SaveNewPatientRecord, CanSaveNewPatientRecord);
            CancelNewPatientRecordCommand = new RelayCommand(window => WindowHelper.CloseCurrentWindow(window), _ => true);
        }

        public void SaveNewPatientRecord(object parameter)
        {
            var patientRecord = new PatientRecord 
            { 
                PatientName = PatientName,
                DoctorName = DoctorName,
                Amount = double.Parse(Amount),
                VisitDate = VisitDate
            };

            Repository.Instance.AddPatientRecord(patientRecord);
            WindowHelper.CloseCurrentWindow(parameter);
        }

        public bool CanSaveNewPatientRecord(object parameter)
        {
            return !string.IsNullOrEmpty(PatientName)
                && !string.IsNullOrEmpty(DoctorName)
                && !string.IsNullOrEmpty(Amount) && double.TryParse(Amount, out _);
        }
    }
}
