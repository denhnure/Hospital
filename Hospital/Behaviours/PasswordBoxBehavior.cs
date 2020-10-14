using System.Windows;
using System.Windows.Controls;
using Hospital.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace Hospital.Behaviours
{
    public sealed class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
            AssociatedObject.DataContextChanged -= AssociatedObject_DataContextChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null && e.NewValue is SignInViewModel)
            {
                ((SignInViewModel)e.NewValue).PasswordReset += SignInViewModel_PasswordReset;
            }
            else if (e.OldValue is SignInViewModel && e.NewValue == null)
            {
                ((SignInViewModel)e.OldValue).PasswordReset -= SignInViewModel_PasswordReset;
            }
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.DataContext is SignInViewModel)
            {
                ((SignInViewModel)AssociatedObject.DataContext).Password = AssociatedObject.Password;
            }
        }

        private void SignInViewModel_PasswordReset(object sender, System.EventArgs e)
        {
            AssociatedObject.Password = string.Empty;
            AssociatedObject.Focus();
        }
    }
}
