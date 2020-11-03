using System;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class PatientReportViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double? amount;

        public PatientReportViewModel()
        {
            CreatePatientReportCommand = new RelayCommand(CreatePatientReport);
        }

        public string PatientName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public double? DoctorAmount => Amount * 0.6;

        public double? HospitalAmount => Amount * 0.4;

        public double? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoctorAmount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HospitalAmount)));
            }
        }

        public ICommand CreatePatientReportCommand { get; private set; }

        private void CreatePatientReport(object obj)
        {
            Amount = Repository.Instance.GetAmount(PatientName, FromDate, ToDate);
        }
    }
}
