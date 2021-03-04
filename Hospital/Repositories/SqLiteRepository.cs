using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using Hospital.Enums;
using Hospital.Models;

namespace Hospital.Repositories
{
    public class SqLiteRepository : IRepository
    {
        private const string PASSWORD = "1";
        private readonly string CONNECTION_STRING;

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
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"INSERT INTO [PatientRecord]
                                    (
                                        [PatientName], [BirthYear], [Gender], [TownOrVillage], [DoctorName], [DoctorAmount], [HospitalAmount], [Amount], [VisitDate]) 
                                        VALUES(@patientName, @birthYear, @gender, @townOrVillage, @doctorName, @doctorAmount, @hospitalAmount, @amount, @visitDate
                                    );";

                    command.Parameters.AddWithValue("@patientName", patientRecord.PatientName);
                    command.Parameters.AddWithValue("@birthYear", patientRecord.BirthYear);
                    command.Parameters.AddWithValue("@gender", (int)patientRecord.Gender);
                    command.Parameters.AddWithValue("@townOrVillage", patientRecord.TownOrVillage);
                    command.Parameters.AddWithValue("@doctorName", patientRecord.DoctorName);
                    command.Parameters.AddWithValue("@doctorAmount", patientRecord.FinancialData.DoctorAmount);
                    command.Parameters.AddWithValue("@hospitalAmount", patientRecord.FinancialData.HospitalAmount);
                    command.Parameters.AddWithValue("@amount", patientRecord.FinancialData.Amount);
                    command.Parameters.AddWithValue("@visitDate", patientRecord.VisitDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public ObservableCollection<PatientRecord> GetPatientRecords(DateTime? date = null)
        {
            var patientRecords = new ObservableCollection<PatientRecord>();

            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT *
                                            FROM [PatientRecord]
                                            WHERE date([VisitDate]) = date(@date) OR @date IS NULL
                                            ORDER BY [VisitDate] DESC";

                    command.Parameters.Add(new SQLiteParameter("@date", date));

                    SQLiteDataReader sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        patientRecords.Add(CreatePatientRecord(sqlDataReader));
                    }
                }
            }

            return patientRecords;
        }

        public ObservableCollection<PatientRecord> GetSpecificPatientRecords(string patientName, DateTime? fromDate, DateTime? toDate)
        {
            var specificPatientRecords = new ObservableCollection<PatientRecord>();

            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT *
                                            FROM [PatientRecord]
                                            WHERE [PatientName] = @patientName
                                                AND (date([VisitDate]) >= date(@fromDate) OR @fromDate IS NULL) 
                                                AND (date([VisitDate]) <= date(@toDate) OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@patientName", patientName));
                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    SQLiteDataReader sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        specificPatientRecords.Add(CreatePatientRecord(sqlDataReader));
                    }
                }
            }

            return specificPatientRecords;
        }

        public PatientRecord GetLastPatientRecord()
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT * FROM [PatientRecord]
                                            WHERE rowid = (SELECT MAX(rowid) FROM [PatientRecord])";

                    SQLiteDataReader sqlDataReader = command.ExecuteReader();
                    sqlDataReader.Read();

                    return CreatePatientRecord(sqlDataReader);
                }
            }
        }

        public void UpdateLastPatientRecord(PatientRecord patientRecord)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"UPDATE [PatientRecord]
                                            SET [PatientName] = @patientName, [BirthYear] = @birthYear, [Gender] = @gender,
                                                [TownOrVillage] = @townOrVillage, [DoctorName] = @doctorName, [DoctorAmount] = @doctorAmount,
                                                [HospitalAmount] = @hospitalAmount, [Amount] = @amount, [VisitDate] = @visitDate
                                            WHERE rowid = (SELECT MAX(rowid) FROM [PatientRecord])";

                    command.Parameters.AddWithValue("@patientName", patientRecord.PatientName);
                    command.Parameters.AddWithValue("@birthYear", patientRecord.BirthYear);
                    command.Parameters.AddWithValue("@gender", (int)patientRecord.Gender);
                    command.Parameters.AddWithValue("@townOrVillage", patientRecord.TownOrVillage);
                    command.Parameters.AddWithValue("@doctorName", patientRecord.DoctorName);
                    command.Parameters.AddWithValue("@doctorAmount", patientRecord.FinancialData.DoctorAmount);
                    command.Parameters.AddWithValue("@hospitalAmount", patientRecord.FinancialData.HospitalAmount);
                    command.Parameters.AddWithValue("@amount", patientRecord.FinancialData.Amount);
                    command.Parameters.AddWithValue("@visitDate", patientRecord.VisitDate);

                    command.ExecuteNonQuery();
                }
            }
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
                                            [BirthYear] INTEGER,
                                            [Gender] INTEGER,
                                            [TownOrVillage] NVARCHAR(200),
                                            [DoctorName] NVARCHAR(200),
                                            [Amount] REAL,
                                            [DoctorAmount] REAL,
                                            [HospitalAmount] REAL,
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

        public PatientRecordFinancialData GetAggregatedPatientFinancialData(string patientName, DateTime? fromDate, DateTime? toDate)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT SUM(DoctorAmount) AS DoctorAmount, SUM(HospitalAmount) AS HospitalAmount, SUM(Amount) AS Amount
                                            FROM [PatientRecord]
                                            WHERE [PatientName] = @patientName
                                                AND (date([VisitDate]) >= date(@fromDate) OR @fromDate IS NULL) 
                                                AND (date([VisitDate]) <= date(@toDate) OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@patientName", patientName));
                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    SQLiteDataReader sqlDataReader = command.ExecuteReader();
                    sqlDataReader.Read();

                    return CreatePatientRecordFinancialData(sqlDataReader);
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
                                            WHERE (date([VisitDate]) >= date(@fromDate) OR @fromDate IS NULL)
                                                AND (date([VisitDate]) <= date(@toDate) OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public PatientRecordFinancialData GetAggregatedFinancialData(DateTime? fromDate, DateTime? toDate)
        {
            using (var con = new SQLiteConnection(CONNECTION_STRING))
            {
                con.Open();

                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = @"SELECT SUM(DoctorAmount) AS DoctorAmount, SUM(HospitalAmount) AS HospitalAmount, SUM(Amount) AS Amount
                                            FROM [PatientRecord]
                                            WHERE (date([VisitDate]) >= date(@fromDate) OR @fromDate IS NULL)
                                                AND (date([VisitDate]) <= date(@toDate) OR @toDate IS NULL)";

                    command.Parameters.Add(new SQLiteParameter("@fromDate", fromDate));
                    command.Parameters.Add(new SQLiteParameter("@toDate", toDate));

                    SQLiteDataReader sqlDataReader = command.ExecuteReader();
                    sqlDataReader.Read();

                    return CreatePatientRecordFinancialData(sqlDataReader);
                }
            }
        }

        private PatientRecord CreatePatientRecord(SQLiteDataReader sqlDataReader)
        {
            return new PatientRecord
            {
                PatientName = (string)sqlDataReader["PatientName"],
                BirthYear = Convert.ToInt32(sqlDataReader["BirthYear"]),
                Gender = (Gender)Convert.ToInt32(sqlDataReader["Gender"]),
                TownOrVillage = (string)sqlDataReader["TownOrVillage"],
                DoctorName = (string)sqlDataReader["DoctorName"],
                FinancialData = CreatePatientRecordFinancialData(sqlDataReader),
                VisitDate = DateTime.Parse((string)sqlDataReader["VisitDate"])
            };
        }

        private PatientRecordFinancialData CreatePatientRecordFinancialData(SQLiteDataReader sqlDataReader)
        {
            return sqlDataReader["Amount"] is DBNull
                ? null
                : new PatientRecordFinancialData
                {
                    DoctorAmount = (double)sqlDataReader["DoctorAmount"],
                    HospitalAmount = (double)sqlDataReader["HospitalAmount"],
                    Amount = (double)sqlDataReader["Amount"],
                };
        }
    }
}
