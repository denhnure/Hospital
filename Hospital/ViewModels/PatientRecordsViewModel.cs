using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.ViewModels
{
    public class PatientRecordsViewModel : IPageViewModel
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public PatientRecordsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddNewPatientRecordCommand = new RelayCommand(AddNewPatientRecord, c => Repository.Instance.IsLoggedIn);
            GoToReportsCommand = new RelayCommand(GoToReports, c => true);
            PatientRecords = Repository.Instance.GetPatientRecords();
        }

        public ICommand AddNewPatientRecordCommand { get; private set; }

        public ICommand GoToReportsCommand { get; private set; }

        public ObservableCollection<PatientRecord> PatientRecords { get; private set; }

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
    }
}
