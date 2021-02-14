using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.ViewModels.PatientRecords
{
    public class EditLastPatientRecordViewModel : BasePatientRecordViewModel, IPageViewModel
    {
        public string Title => "Edit";

        public EditLastPatientRecordViewModel(MainWindowViewModel mainWindowViewModel)
            : base(mainWindowViewModel)
        {
            PatientRecord lastPatientRecord = Repository.Instance.GetLastPatientRecord();
            
            PatientName = lastPatientRecord.PatientName;
            DoctorName = lastPatientRecord.DoctorName;
            Amount = lastPatientRecord.Amount.ToString();
            VisitDate = lastPatientRecord.VisitDate;
        }

        protected override void SaveChangesToRepository(PatientRecord patientRecord)
        {
            Repository.Instance.UpdateLastPatientRecord(patientRecord);
        }
    }
}