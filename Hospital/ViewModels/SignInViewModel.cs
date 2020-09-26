using System.ComponentModel;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Repositories;

namespace Hospital.ViewModels
{
    public class SignInViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MainWindowViewModel mainWindowViewModel;
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public ICommand SignInCommand { get; private set; }

        public SignInViewModel(MainWindowViewModel mainWindowViewModel)
        {
            SignInCommand = new RelayCommand(SignIn, _ => !string.IsNullOrEmpty(Password));
            Password = string.Empty;
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public void SignIn(object parameter)
        {
            Repository.Instance.Login(Password);

            if (Repository.Instance.IsLoggedIn)
            {
                mainWindowViewModel.CurrentPageViewModel = new PatientRecordsViewModel(mainWindowViewModel);
            }
        }
    }
}
