using System;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Models;

namespace Hospital.ViewModels.PatientRecords
{
    public abstract class BasePatientRecordViewModel
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Amount { get; set; }

        public DateTime VisitDate { get; set; }

        public ICommand SavePatientRecordCommand { get; private set; }

        protected BasePatientRecordViewModel(MainWindowViewModel mainWindowViewModel)
        {
            SavePatientRecordCommand = new RelayCommand(SavePatientRecord, CanSavePatientRecord);
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public void SavePatientRecord(object parameter)
        {
            var patientRecord = new PatientRecord
            {
                PatientName = PatientName,
                DoctorName = DoctorName,
                Amount = double.Parse(Amount),
                VisitDate = VisitDate.Date
            };

            SaveChangesToRepository(patientRecord);
            mainWindowViewModel.GoToPatientRecordsCommand.Execute(null);
        }

        protected abstract void SaveChangesToRepository(PatientRecord patientRecord);

        public bool CanSavePatientRecord(object parameter)
        {
            double parsedAmount;

            return !string.IsNullOrEmpty(PatientName)
                && !string.IsNullOrEmpty(DoctorName)
                && !string.IsNullOrEmpty(Amount) && double.TryParse(Amount, out parsedAmount) && parsedAmount > 0
                && VisitDate.Date <= DateTime.Today;
        }
    }
}