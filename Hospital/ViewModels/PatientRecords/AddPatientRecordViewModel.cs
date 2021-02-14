using System;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.ViewModels.PatientRecords
{
    public class AddPatientRecordViewModel : BasePatientRecordViewModel, IPageViewModel
    {
        public string Title => "Add";

        public AddPatientRecordViewModel(MainWindowViewModel mainWindowViewModel)
            : base(mainWindowViewModel)
        {
            VisitDate = DateTime.Now;
        }

        protected override void SaveChangesToRepository(PatientRecord patientRecord)
        {
            Repository.Instance.AddPatientRecord(patientRecord);
        }
    }
}