using System.Windows;
using System.Windows.Controls;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((SignInViewModel)DataContext).Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
