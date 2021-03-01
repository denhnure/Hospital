using System;
using Hospital.Enums;

namespace Hospital.Models
{
    public class PatientRecord
    {
        public string PatientName { get; set; }

        public int BirthYear { get; set; }

        public Gender Gender { get; set; }

        public string TownOrVillage { get; set; }

        public string DoctorName { get; set; }

        public PatientRecordFinancialData FinancialData { get; set; }

        public DateTime VisitDate { get; set; }
    }
}
