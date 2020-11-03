using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;

namespace Hospital.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IPageViewModel currentPageViewModel;

        public MainWindowViewModel()
        {
            GoToPatientRecordsCommand = new RelayCommand(GoToPatientRecords);
            CurrentPageViewModel = new ReportsViewModel();
        }

        public ICommand GoToPatientRecordsCommand { get; private set; }

        public IPageViewModel CurrentPageViewModel
        {
            get { return currentPageViewModel; }
            set
            {
                currentPageViewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPageViewModel)));
            }
        }

        private void GoToPatientRecords(object parameter)
        {
            CurrentPageViewModel = new PatientRecordsViewModel(this);
        }
    }
}
