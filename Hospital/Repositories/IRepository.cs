using System.Collections.ObjectModel;
using Hospital.Models;

namespace Hospital.Repositories
{
    public interface IRepository
    {
        void AddPatientRecord(PatientRecord patientRecord);

        ObservableCollection<PatientRecord> GetPatientRecords();
    }
}
