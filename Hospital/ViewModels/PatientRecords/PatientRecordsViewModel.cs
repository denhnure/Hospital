using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Models;
using Hospital.Properties;
using Hospital.Repositories;
using Hospital.ViewModels.Reports;

namespace Hospital.ViewModels.PatientRecords
{
    public class PatientRecordsViewModel : IPageViewModel, INotifyPropertyChanged
    {
        private const int MINUMUM_SPINNER_DURATION = 1;
        private readonly MainWindowViewModel mainWindowViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        private bool isFetchingPatientRecords;
        private ObservableCollection<PatientRecord> patientRecords;

        public string Title => Resources.PatientRecords;

        public ICommand AddNewPatientRecordCommand { get; private set; }

        public ICommand EditLastPatientRecordCommand { get; private set; }

        public ICommand GoToReportsCommand { get; private set; }

        public bool IsFetchingPatientRecords
        {
            get { return isFetchingPatientRecords; }
            private set
            {
                isFetchingPatientRecords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFetchingPatientRecords)));
            }
        }

        public ObservableCollection<PatientRecord> PatientRecords
        { 
            get { return patientRecords; }
            private set
            {
                patientRecords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PatientRecords)));
            }
        }

        public PatientRecordsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddNewPatientRecordCommand = new RelayCommand(AddNewPatientRecord, c => Repository.Instance.IsLoggedIn);
            EditLastPatientRecordCommand = new RelayCommand(EditLastPatientRecord, c => Repository.Instance.IsLoggedIn && PatientRecords.Count > 0);
            GoToReportsCommand = new RelayCommand(GoToReports);

            FetchPatientRecords();
        }

        private void AddNewPatientRecord(object parameter)
        {
            mainWindowViewModel.CurrentPageViewModel = new AddPatientRecordViewModel(mainWindowViewModel);
        }

        private void EditLastPatientRecord(object parameter)
        {
            mainWindowViewModel.CurrentPageViewModel = new EditLastPatientRecordViewModel(mainWindowViewModel);
        }

        private void GoToReports(object parameter)
        {
            mainWindowViewModel.CurrentPageViewModel = new ReportsViewModel();
        }

        private async void FetchPatientRecords()
        {
            IsFetchingPatientRecords = true;

            Task loadingSpinnerTask = Task.Delay(MINUMUM_SPINNER_DURATION);
            Task getPatientRecordsTask = Task.Run(() => PatientRecords = Repository.Instance.GetPatientRecords());

            await Task.WhenAll(loadingSpinnerTask, getPatientRecordsTask);
            
            IsFetchingPatientRecords = false;
        }
    }
}
