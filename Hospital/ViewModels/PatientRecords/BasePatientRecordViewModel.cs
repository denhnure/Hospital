using System;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Enums;
using Hospital.Models;

namespace Hospital.ViewModels.PatientRecords
{
    public abstract class BasePatientRecordViewModel
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public string PatientName { get; set; }

        public int? BirthYear { get; set; }

        public Gender Gender { get; set; }

        public string TownOrVillage { get; set; }

        public string DoctorName { get; set; }

        public double? DoctorAmount { get; set; }

        public double? HospitalAmount { get; set; }

        public double? Amount { get; set; }

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
                    DoctorAmount = DoctorAmount.Value,
                    HospitalAmount = HospitalAmount.Value,
                    Amount = Amount.Value
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
                && BirthYear.HasValue && BirthYear.Value >= Constants.OLDEST_PERSON_YEAR_OF_BIRTH
                && !string.IsNullOrEmpty(DoctorName)
                && IsValidAmount(DoctorAmount)
                && IsValidAmount(HospitalAmount)
                && IsValidAmount(Amount)
                && OverallAmountMatchesToDoctorAndHospitalAmounts()
                && VisitDate.Date <= DateTime.Today;
        }

        private bool IsValidAmount(double? amount)
        {
            return amount.HasValue && amount.Value >= 0;
        }

        private bool OverallAmountMatchesToDoctorAndHospitalAmounts()
        {
            return Math.Abs(DoctorAmount.Value + HospitalAmount.Value - Amount.Value) < 0.0000001;
        }
    }
}