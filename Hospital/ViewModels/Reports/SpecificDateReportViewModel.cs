using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.ViewModels.Reports
{
    public class SpecificDateReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<PatientRecord> patientRecords;

        public SpecificDateReportViewModel()
        {
            CreateSpecificDateReportCommand = new RelayCommand(CreateSpecificDateReport, _ => Date <= DateTime.Today);
        }

        public DateTime? Date { get; set; }

        public ObservableCollection<PatientRecord> PatientRecords 
        {
            get { return patientRecords; }
            private set
            {
                patientRecords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PatientRecords)));
            }
        }

        public ICommand CreateSpecificDateReportCommand { get; private set; }

        private void CreateSpecificDateReport(object obj)
        {
            PatientRecords = Repository.Instance.GetPatientRecords(Date);
        }
    }
}
