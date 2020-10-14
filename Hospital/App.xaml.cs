using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Hospital.Repositories;

namespace Hospital
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Repository.Instance = new SqLiteRepository();
            SetCulture();
        }

        private void SetCulture()
        {
            CultureInfo cultureInfo = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            FrameworkElement.LanguageProperty.OverrideMetadata
            (
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag))
            );
        }
    }
}
