using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.ViewModels
{
    public class PatientRecordsViewModel : IPageViewModel, INotifyPropertyChanged
    {
        private const int MINUMUM_SPINNER_DURATION = 1000;
        private readonly MainWindowViewModel mainWindowViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        private bool isFetchingPatientRecords;
        private ObservableCollection<PatientRecord> patientRecords;

        public PatientRecordsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddNewPatientRecordCommand = new RelayCommand(AddNewPatientRecord, c => Repository.Instance.IsLoggedIn);
            GoToReportsCommand = new RelayCommand(GoToReports);

            FetchPatientRecords();
        }

        public ICommand AddNewPatientRecordCommand { get; private set; }

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

        private void AddNewPatientRecord(object parameter)
        {
            var addNewPatientWindow = new AddNewPatientWindow();
            addNewPatientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addNewPatientWindow.Owner = Application.Current.MainWindow;
            addNewPatientWindow.DataContext = new AddNewPatientRecordViewModel();
            addNewPatientWindow.ShowDialog();
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
