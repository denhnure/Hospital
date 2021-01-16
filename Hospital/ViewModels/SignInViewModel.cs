using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Hospital.Commands;
using Hospital.Enums;
using Hospital.Repositories;

namespace Hospital.ViewModels
{
    public class SignInViewModel : IPageViewModel, INotifyPropertyChanged
    {
        private const int LOGIN_DELAY_IN_MILLISECONDS = 1000;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler PasswordReset;
        private readonly MainWindowViewModel mainWindowViewModel;
        private LoginStatus loginStatus;
        private string password;

        public bool IsNotLoggedIn => LoginStatus == LoginStatus.NOT_LOGGED_IN;

        public bool WrongCredentials => LoginStatus == LoginStatus.WRONG_CREDENTIALS;

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

        public ICommand TryLoginAgainCommand { get; private set; }

        private LoginStatus LoginStatus
        {
            get { return loginStatus; }
            set 
            {
                loginStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotLoggedIn)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WrongCredentials)));
            }
        }

        public SignInViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            LoginStatus = LoginStatus.NOT_LOGGED_IN;
            ResetPassword();

            SignInCommand = new RelayCommand(SignIn, _ => true);
            TryLoginAgainCommand = new RelayCommand(TryLoginAgain, _ => true);
        }

        public void SignIn(object parameter)
        {
            Repository.Instance.Login(Password);

            if (!Repository.Instance.IsLoggedIn)
            {
                LoginStatus = LoginStatus.WRONG_CREDENTIALS;
                return;
            }
            
            mainWindowViewModel.CurrentPageViewModel = new PatientRecordsViewModel(mainWindowViewModel);
        }

        public void TryLoginAgain(object parameter)
        {
            LoginStatus = LoginStatus.NOT_LOGGED_IN;
            ResetPassword();
        }

        private void ResetPassword()
        {
            Password = string.Empty;
            PasswordReset?.Invoke(this, null);
        }
    }
}
