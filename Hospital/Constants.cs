using Hospital.Properties;

namespace Hospital
{
    public static class Constants
    {
        public const string APPLICATION_NAME = "Nevromed";
        public const double DOCTOR_AMOUNT_FACTOR = 0.6;
        public const double HOSPITAL_AMOUNT_FACTOR = 0.4;
        public static readonly string DOCTOR_AMOUNT_TEXT = string.Format(Resources.DoctorAmount, DOCTOR_AMOUNT_FACTOR * PERCENTAGE_FACTOR);
        public static readonly string HOSPITAL_AMOUNT_TEXT = string.Format(Resources.HospitalAmount, HOSPITAL_AMOUNT_FACTOR * PERCENTAGE_FACTOR);

        private const int PERCENTAGE_FACTOR = 100;
    }
}
