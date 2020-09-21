using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using Hospital.Models;

namespace Hospital.Repositories
{
    public class SqLiteRepository : IRepository
    {
        private const string CONNECTION_STRING = "Data Source=database.sqlite";

        private ObservableCollection<PatientRecord> patientRecords = new ObservableCollection<PatientRecord>();

        public SqLiteRepository()
        {
            CreatePatientRecordTable();
        }

        public void AddPatientRecord(PatientRecord patientRecord)
        {
            patientRecords.Insert(0, patientRecord);

            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"INSERT INTO [PatientRecord]
                                        (
                                            [PatientName], [DoctorName], [Amount], [VisitDate]) 
                                            VALUES(@patientName, @doctorName, @amount, @visitDate
                                        );";

                    command.Parameters.Add(new SQLiteParameter("@patientName", patientRecord.PatientName));
                    command.Parameters.Add(new SQLiteParameter("@doctorName", patientRecord.DoctorName));
                    command.Parameters.Add(new SQLiteParameter("@amount", patientRecord.Amount));
                    command.Parameters.Add(new SQLiteParameter("@visitDate", patientRecord.VisitDate));

                    command.ExecuteNonQuery();
                }
            }
        }

        public ObservableCollection<PatientRecord> GetPatientRecords()
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT * FROM [PatientRecord]
                                            ORDER BY [VisitDate] DESC;";

                    SQLiteDataReader sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        patientRecords.Add
                        (
                            new PatientRecord 
                            { 
                                PatientName = (string)sqlDataReader["PatientName"],
                                DoctorName = (string)sqlDataReader["DoctorName"],
                                Amount = (double)sqlDataReader["Amount"],
                                VisitDate = DateTime.Parse((string)sqlDataReader["VisitDate"])
                            }
                        );
                    }
                }
            }

            return patientRecords;
        }

        private void CreatePatientRecordTable()
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [PatientRecord]
                                        (
                                            [PatientName] NVARCHAR(200),
                                            [DoctorName] NVARCHAR(200),
                                            [Amount] REAL,
                                            [VisitDate] TEXT
                                        );";
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
