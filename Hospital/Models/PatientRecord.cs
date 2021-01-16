using System;

namespace Hospital.Models
{
    public class PatientRecord
    {
        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public double DoctorAmount => Amount * Constants.DOCTOR_AMOUNT_FACTOR;

        public double HospitalAmount => Amount * Constants.HOSPITAL_AMOUNT_FACTOR;

        public double Amount { get; set; }

        public DateTime VisitDate { get; set; }
    }
}
