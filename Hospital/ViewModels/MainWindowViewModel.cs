using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            PatientRecords = Repository.Instance.GetPatientRecords();
            AddNewPatientRecordCommand = new RelayCommand(AddNewPatientRecord, c => true);
        }

        public ICommand AddNewPatientRecordCommand { get; private set; }

        public ObservableCollection<PatientRecord> PatientRecords { get; private set; }

        public void AddNewPatientRecord(object parameter)
        {
            AddNewPatientWindow addNewPatientWindow = new AddNewPatientWindow();
            addNewPatientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addNewPatientWindow.Owner = Application.Current.MainWindow;
            addNewPatientWindow.DataContext = new AddNewPatientRecordViewModel();
            addNewPatientWindow.ShowDialog();
        }
    }
}
