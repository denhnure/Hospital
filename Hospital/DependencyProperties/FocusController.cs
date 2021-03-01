using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hospital.DependencyProperties
{
    public static class FocusController
    {
        public static readonly DependencyProperty FocusFirstControlProperty =
            DependencyProperty.RegisterAttached(
                "FocusFirstControl",
                typeof(bool),
                typeof(FocusController),
                new PropertyMetadata(false, OnPropertyChanged));

        public static bool GetFocusFirstControl(Control control)
        {
            return (bool)control.GetValue(FocusFirstControlProperty);
        }

        public static void SetFocusFirstControl(Control control, bool value)
        {
            control.SetValue(FocusFirstControlProperty, value);
        }

        static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Control control = obj as Control;
            if (control == null || !(args.NewValue is bool) || !(bool)args.NewValue)
            {
                return;
            }

            control.Loaded += (sender, e) => control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
