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

        ObservableCollection<PatientRecord> GetPatientRecords(DateTime? date = null);

        PatientRecord GetLastPatientRecord();

        void UpdateLastPatientRecord(PatientRecord patientRecord);

        bool DoesPatientExist(string patientName);

        double? GetPatientAmount(string patientName, DateTime? fromDate, DateTime? toDate);

        int GetPatientCount(DateTime? fromDate, DateTime? toDate);

        double? GetAmount(DateTime? fromDate, DateTime? toDate);
    }
}
