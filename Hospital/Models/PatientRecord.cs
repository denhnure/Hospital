using System;

namespace Hospital.Models
{
    public class PatientRecord
    {
        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public double Amount { get; set; }

        public DateTime VisitDate { get; set; }
    }
}
