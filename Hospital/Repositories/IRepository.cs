using System;
using System.Collections.ObjectModel;
using Hospital.Models;

namespace Hospital.Repositories
{
    public interface IRepository
    {
        bool IsLoggedIn { get; }
        
        void Login(string password);
        
        void AddPatientRecord(PatientRecord patientRecord);

        ObservableCollection<PatientRecord> GetPatientRecords();

        bool DoesPatientExist(string patientName);

        double? GetAmount(string patientName, DateTime? fromDate, DateTime? toDate);
    }
}
