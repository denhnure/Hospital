using System;
using System.Collections.ObjectModel;
using Hospital.Models;

namespace Hospital.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private ObservableCollection<PatientRecord> patientRecords = new ObservableCollection<PatientRecord>();

        public bool IsLoggedIn => true;

        public InMemoryRepository()
        {
            AddPatientRecord(new PatientRecord { PatientName = "Den", DoctorName = "Rishar", Amount = 10, VisitDate = new DateTime(2020, 09, 18) });
            AddPatientRecord(new PatientRecord { PatientName = "Kseniia", DoctorName = "Rishar", Amount = 20, VisitDate = new DateTime(2020, 09, 18) });
        }

        public void Login(string password)
        {
        }

        public void AddPatientRecord(PatientRecord patientRecord)
        {
            patientRecords.Insert(0, patientRecord);
        }

        public ObservableCollection<PatientRecord> GetPatientRecords()
        {
            return patientRecords;
        }

        public bool DoesPatientExist(string patientName)
        {
            return false;
        }

        public double? GetPatientAmount(string patientName, DateTime? fromDate, DateTime? toDate)
        {
            return 0;
        }

        public int GetPatientCount(DateTime? fromDate, DateTime? toDate)
        {
            return 0;
        }

        public double? GetAmount(DateTime? fromDate, DateTime? toDate)
        {
            return 0;
        }
    }
}
