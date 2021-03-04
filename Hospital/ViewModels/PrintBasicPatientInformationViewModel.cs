using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Hospital.Commands;
using Hospital.Properties;

namespace Hospital.ViewModels
{
    public class PrintBasicPatientInformationViewModel : IPageViewModel
    {
        private const int PRECREATION_OF_FIXED_PAGE_DELAY = 100;
        private const double SYMBOLS_PER_PAGE = 120;
        private const string FONT_FAMILY = "Arial";
        private const int FONT_SIZE = 14;

        private FixedPage fixedPage;

        public string Title => Resources.PrintBasicPatientInformation;

        public ICommand PrintCommand { get; private set; }

        public string PatientName { get; set; }

        public int? BirthYear { get; set; }

        private PrintDialog PrintDialog { get; set; }

        public PrintBasicPatientInformationViewModel()
        {
            Task.Delay(PRECREATION_OF_FIXED_PAGE_DELAY).ContinueWith(_ => Application.Current.Dispatcher.Invoke(() => CreateFixedPageWithHeader()));

            PrintCommand = new RelayCommand(Print, CanPrint);
            PrintDialog = new PrintDialog();
        }

        private void Print(object obj)
        {
            fixedPage.Children.Add(CreateTextBlock($"{PatientName} {BirthYear} {Resources.BirthYearShortForm}", 155));

            FixedDocument fixedDocument = CreateAndPopulateFixedDocument();
            PrintDialog.PrintDocument(fixedDocument.DocumentPaginator, Resources.PrintBasicPatientInformation);

            CreateFixedPageWithHeader();
        }

        private bool CanPrint(object parameter)
        {
            return !string.IsNullOrEmpty(PatientName)
                && BirthYear.HasValue
                && BirthYear.Value >= Constants.OLDEST_PERSON_YEAR_OF_BIRTH;
        }

        private FixedDocument CreateAndPopulateFixedDocument()
        {
            FixedDocument fixedDocument = CreateBlankFixedDocument();
            fixedDocument.Pages.Add(CreateAndPopulatePageContent());

            return fixedDocument;
        }

        private FixedDocument CreateBlankFixedDocument()
        {
            FixedDocument fixedDocument = new FixedDocument();
            fixedDocument.DocumentPaginator.PageSize = new Size(PrintDialog.PrintableAreaWidth, PrintDialog.PrintableAreaHeight);

            return fixedDocument;
        }

        private PageContent CreateAndPopulatePageContent()
        {
            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);

            return pageContent;
        }

        private void CreateFixedPageWithHeader()
        {
            fixedPage = new FixedPage()
            {
                Width = PrintDialog.PrintableAreaWidth,
                Height = PrintDialog.PrintableAreaHeight
            };

            foreach (var textBlock in CreateTextBlocksForHeader())
            {
                fixedPage.Children.Add(textBlock);
            }
        }

        private List<TextBlock> CreateTextBlocksForHeader()
        {
            return new List<TextBlock>
            {
                CreateTextBlock(Resources.FullCompanyName, 50),
                CreateTextBlock(Resources.MinistryOfHealthCareLicense, 75),
                CreateTextBlock(Resources.CompanyAddress, 100)
            };
        }

        private TextBlock CreateTextBlock(string text, double y)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontFamily = new FontFamily(FONT_FAMILY);
            textBlock.FontSize = FONT_SIZE;
            textBlock.FontStretch = FontStretches.Expanded;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Margin = new Thickness(CalculateHorizontalCenterOfPrintableArea(text), y, 0, 0);
            textBlock.Padding = new Thickness(2);

            return textBlock;
        }

        private double CalculateHorizontalCenterOfPrintableArea(string text)
        {
            double horizontalCenterOfPrintableArea = PrintDialog.PrintableAreaWidth / 2;
            
            return horizontalCenterOfPrintableArea - horizontalCenterOfPrintableArea * (text.Length / SYMBOLS_PER_PAGE);
        }
    }
}
