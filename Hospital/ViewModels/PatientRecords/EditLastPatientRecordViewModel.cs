using Hospital.Models;
using Hospital.Properties;
using Hospital.Repositories;

namespace Hospital.ViewModels.PatientRecords
{
    public class EditLastPatientRecordViewModel : BasePatientRecordViewModel, IPageViewModel
    {
        public string Title => Resources.EditLastPatientRecord;

        public EditLastPatientRecordViewModel(MainWindowViewModel mainWindowViewModel)
            : base(mainWindowViewModel)
        {
            PatientRecord lastPatientRecord = Repository.Instance.GetLastPatientRecord();

            PatientName = lastPatientRecord.PatientName;
            BirthYear = lastPatientRecord.BirthYear;
            Gender = lastPatientRecord.Gender;
            TownOrVillage = lastPatientRecord.TownOrVillage;
            DoctorName = lastPatientRecord.DoctorName;

            DoctorAmount = lastPatientRecord.FinancialData.DoctorAmount;
            HospitalAmount = lastPatientRecord.FinancialData.HospitalAmount;
            Amount = lastPatientRecord.FinancialData.Amount;
            VisitDate = lastPatientRecord.VisitDate;
        }

        protected override void SaveChangesToRepository(PatientRecord patientRecord)
        {
            Repository.Instance.UpdateLastPatientRecord(patientRecord);
        }
    }
}