using System;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Enums;
using Hospital.Models;

namespace Hospital.ViewModels.PatientRecords
{
    public abstract class BasePatientRecordViewModel
    {
        private const int OLDEST_PERSON_YEAR_OF_BIRTH = 1900;
        private readonly MainWindowViewModel mainWindowViewModel;

        public string PatientName { get; set; }

        public int? BirthYear { get; set; }

        public Gender Gender { get; set; }

        public string TownOrVillage { get; set; }

        public string DoctorName { get; set; }

        public string Amount { get; set; }

        public string DoctorAmount { get; set; }

        public string HospitalAmount { get; set; }

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
                BirthYear = BirthYear.Value,
                Gender = Gender,
                TownOrVillage = TownOrVillage,
                DoctorName = DoctorName,
                FinancialData = new PatientRecordFinancialData
                {
                    DoctorAmount = double.Parse(DoctorAmount),
                    HospitalAmount = double.Parse(HospitalAmount),
                    Amount = double.Parse(Amount)
                },
                VisitDate = VisitDate.Date
            };

            SaveChangesToRepository(patientRecord);
            mainWindowViewModel.GoToPatientRecordsCommand.Execute(null);
        }

        protected abstract void SaveChangesToRepository(PatientRecord patientRecord);

        public bool CanSavePatientRecord(object parameter)
        {
            return !string.IsNullOrEmpty(PatientName)
                && BirthYear.HasValue && BirthYear.Value >= OLDEST_PERSON_YEAR_OF_BIRTH
                && !string.IsNullOrEmpty(DoctorName)
                && IsValidAmount(DoctorAmount)
                && IsValidAmount(HospitalAmount)
                && IsValidAmount(Amount)
                && VisitDate.Date <= DateTime.Today;
        }

        private bool IsValidAmount(string amount)
        {
            double parsedAmount;
            
            return !string.IsNullOrEmpty(amount) && double.TryParse(amount, out parsedAmount) && parsedAmount > 0;
        }
    }
}