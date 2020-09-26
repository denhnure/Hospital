using System.Windows;

namespace Hospital.Helpers
{
    public static class WindowHelper
    {
        public static void CloseCurrentWindow(object window)
        {
            Window addNewPatientRecordWindow = window as Window;
            addNewPatientRecordWindow?.Close();
        }
    }
}
