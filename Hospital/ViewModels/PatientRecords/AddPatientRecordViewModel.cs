using System;
using Hospital.Enums;
using Hospital.Models;
using Hospital.Properties;
using Hospital.Repositories;

namespace Hospital.ViewModels.PatientRecords
{
    public class AddPatientRecordViewModel : BasePatientRecordViewModel, IPageViewModel
    {
        public string Title => Resources.AddNewPatientRecord;

        public AddPatientRecordViewModel(MainWindowViewModel mainWindowViewModel)
            : base(mainWindowViewModel)
        {
            VisitDate = DateTime.Now;
            Gender = Gender.Male;
        }

        protected override void SaveChangesToRepository(PatientRecord patientRecord)
        {
            Repository.Instance.AddPatientRecord(patientRecord);
        }
    }
}