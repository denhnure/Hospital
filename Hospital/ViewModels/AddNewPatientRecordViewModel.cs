using System;
using System.Windows;
using System.Windows.Input;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital
{
    public class AddNewPatientRecordViewModel
    {
        public AddNewPatientRecordViewModel()
        {
            VisitDate = DateTime.Now;
            SaveNewPatientRecordCommand = new RelayCommand(SaveNewPatientRecord, CanSaveNewPatientRecord);
            CancelNewPatientRecordCommand = new RelayCommand(CancelNewPatientRecordExecute, c => true);
        }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Amount { get; set; }

        public DateTime VisitDate { get; set; }

        public ICommand SaveNewPatientRecordCommand { get; private set; }

        public ICommand CancelNewPatientRecordCommand { get; private set; }

        public bool CanSaveNewPatientRecord(object parameter)
        {
            return !string.IsNullOrEmpty(PatientName)
                && !string.IsNullOrEmpty(DoctorName)
                && !string.IsNullOrEmpty(Amount) && double.TryParse(Amount, out _);
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

            CloseCurrentWindow(parameter);
        }

        public void CancelNewPatientRecordExecute(object parameter)
        {
            CloseCurrentWindow(parameter);
        }

        private void CloseCurrentWindow(object window)
        {
            Window addNewPatientRecordWindow = window as Window;
            addNewPatientRecordWindow?.Close();
        }
    }
}
