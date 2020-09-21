using System.Windows;
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
        }
    }
}
