using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using Hospital.Models;

namespace Hospital.Repositories
{
    public class SqLiteRepository : IRepository
    {
        private const string PASSWORD = "1";
        private readonly string CONNECTION_STRING;

        private ObservableCollection<PatientRecord> patientRecords;
        private string password;

        public bool IsLoggedIn => password == PASSWORD;

        public SqLiteRepository()
        {
            CONNECTION_STRING = BuildConnectionString();
            CreatePatientRecordTable();
        }
        private string BuildConnectionString()
        {
            string commonApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string databaseFolder = $"{commonApplicationDataFolder}{Path.DirectorySeparatorChar}{Constants.APPLICATION_NAME}{Path.DirectorySeparatorChar}";

            if (!Directory.Exists(databaseFolder))
            {
                Directory.CreateDirectory(databaseFolder);
            }

            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.DataSource = $"{databaseFolder}database.sqlite";

            return connectionStringBuilder.ConnectionString;
        }

        public void Login(string password)
        {
            this.password = password;
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

                    command.Parameters.AddWithValue("@patientName", patientRecord.PatientName);
                    command.Parameters.AddWithValue("@doctorName", patientRecord.DoctorName);
                    command.Parameters.AddWithValue("@amount", patientRecord.Amount);
                    command.Parameters.AddWithValue("@visitDate", patientRecord.VisitDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public ObservableCollection<PatientRecord> GetPatientRecords(DateTime? date = null)
        {
            patientRecords = new ObservableCollection<PatientRecord>();

            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT *
                                            FROM [PatientRecord]
                                            WHERE [VisitDate] == @date OR @date IS NULL
                                            ORDER BY [VisitDate] DESC";

                    command.Parameters.Add(new SQLiteParameter("@date", date));

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

        public bool DoesPatientExist(string patientName)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT 1
                                            FROM [PatientRecord]
                                            WHERE [PatientName] = @patientName";

                    command.Parameters.Add(new SQLiteParameter("@patientName", patientName));

                    return command.ExecuteScalar() != null;
                }
            }
        }

        public double? GetPatientAmount(string patientName, DateTime? fromDate, DateTime? toDate)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT SUM(Amount)
                                            FROM [PatientRecord]
                                            WHERE [PatientName] = @patientName
                                                AND ([VisitDate] >= @fromDate OR @fromDate IS NULL) 
                                                AND ([VisitDate] <= @toDate OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@patientName", patientName));
                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    var result = command.ExecuteScalar();

                    return result is DBNull
                        ? null
                        : (double?)result;
                }
            }
        }

        public int GetPatientCount(DateTime? fromDate, DateTime? toDate)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT COUNT(*)
                                            FROM [PatientRecord]
                                            WHERE ([VisitDate] >= @fromDate OR @fromDate IS NULL)
                                                AND ([VisitDate] <= @toDate OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public double? GetAmount(DateTime? fromDate, DateTime? toDate)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT SUM(Amount)
                                            FROM [PatientRecord]
                                            WHERE ([VisitDate] >= @fromDate OR @fromDate IS NULL)
                                                AND ([VisitDate] <= @toDate OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    var result = command.ExecuteScalar();

                    return result is DBNull
                        ? null
                        : (double?)result;
                }
            }
        }
    }
}
