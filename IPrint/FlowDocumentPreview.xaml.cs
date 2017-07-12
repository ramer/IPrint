using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace IPrint
{
    public partial class FlowDocumentPreview : Window
    {
        public FlowDocument fd;
        public string description;
        public bool singlecolumn;

        private PrintDialog pd = new PrintDialog();

        public FlowDocumentPreview()
        {
            InitializeComponent();
        }

        private void Preview_Loaded(object sender, RoutedEventArgs e)
        {
            stpPrint.DataContext = pd;

            GetPrinters();
            GetOrientations();
        }

        public void GetPrinters()
        {
            List<PrintQueue> printers = new List<PrintQueue>();
            LocalPrintServer printServer = new LocalPrintServer();

            printers.AddRange(printServer.GetPrintQueues(new EnumeratedPrintQueueTypes[] { EnumeratedPrintQueueTypes.Connections }));
            printers.AddRange(printServer.GetPrintQueues(new EnumeratedPrintQueueTypes[] { EnumeratedPrintQueueTypes.Local }));

            cmboPrinter.ItemsSource = printers;

            foreach (PrintQueue printer in printers)
            {
                if (pd.PrintQueue.FullName.Equals(printer.FullName))
                {
                    pd.PrintQueue = printer;
                    cmboPrinter.SelectedItem = printer;
                }
            }
        }

        private void GetOrientations()
        {
            Dictionary<PageOrientation, string> orientations = new Dictionary<PageOrientation, string>();

            cmboOrientation.ItemsSource = new Dictionary<PageOrientation, string>()
            {
                { PageOrientation.Portrait, Properties.Resources.OrientationPortrait },
                { PageOrientation.Landscape, Properties.Resources.OrientationLandscape }
            };
        }

        private void GetDuplexing()
        {
            ReadOnlyCollection<Duplexing> allowedduplex = pd.PrintQueue.GetPrintCapabilities().DuplexingCapability;
            Dictionary<Duplexing, string> duplex = new Dictionary<Duplexing, string>();

            if (allowedduplex.Contains(Duplexing.OneSided)) { duplex.Add(Duplexing.OneSided, Properties.Resources.DuplexOneSided); }
            if (allowedduplex.Contains(Duplexing.TwoSidedLongEdge)) { duplex.Add(Duplexing.TwoSidedLongEdge, Properties.Resources.DuplexTwoSidedLongEdge); }
            if (allowedduplex.Contains(Duplexing.TwoSidedShortEdge)) { duplex.Add(Duplexing.TwoSidedShortEdge, Properties.Resources.DuplexTwoSidedShortEdge); }

            cmboDuplexing.ItemsSource = duplex;
            cmboDuplexing.IsEnabled = duplex.Count > 0;

            if (duplex.Count > 0) {
                pd.PrintTicket.Duplexing = duplex.Keys.First();
                cmboDuplexing.SelectedItem = duplex.Keys.First();
            }
        }

        private void PreparePreview()
        {
            FlowDocument previewfd = IPrintDialog.FlowDocumentClone(fd);
            PageImageableArea area = pd.PrintQueue.GetPrintCapabilities().PageImageableArea;

            if (area != null && area != null)
            {
                if (pd.PrintTicket.PageOrientation == PageOrientation.Portrait)
                {
                    previewfd.PageWidth = area.ExtentWidth + area.OriginWidth * 2;
                    previewfd.PageHeight = area.ExtentHeight + area.OriginHeight * 2;
                }
                else if (pd.PrintTicket.PageOrientation == PageOrientation.Landscape)
                {
                    previewfd.PageWidth = area.ExtentHeight + area.OriginHeight * 2;
                    previewfd.PageHeight = area.ExtentWidth + area.OriginWidth * 2;
                }
            }

            if (singlecolumn)
            {
                previewfd.ColumnGap = 0;
                previewfd.ColumnWidth = pd.PrintableAreaWidth;
            }

            fd.Background = Brushes.White;
            fd.Foreground = Brushes.Black;

            DocumentPaginator previewpaginator = ((IDocumentPaginatorSource)previewfd).DocumentPaginator;
            previewpaginator.ComputePageCount();
            rnPageCount.Text = previewpaginator.PageCount.ToString();
            fdpvPreview.Document = previewfd;
        }

        private void PreparePrint()
        {
            FlowDocument printfd = IPrintDialog.FlowDocumentClone(fd);
            PageImageableArea area = pd.PrintQueue.GetPrintCapabilities().PageImageableArea;

            if (area != null && area != null)
            {
                if (pd.PrintTicket.PageOrientation == PageOrientation.Portrait)
                {
                    printfd.PageWidth = area.ExtentWidth + area.OriginWidth * 2;
                    printfd.PageHeight = area.ExtentHeight + area.OriginHeight * 2;
                }
                else if (pd.PrintTicket.PageOrientation == PageOrientation.Landscape)
                {
                    printfd.PageWidth = area.ExtentHeight + area.OriginHeight * 2;
                    printfd.PageHeight = area.ExtentWidth + area.OriginWidth * 2;
                }
            }

            if (singlecolumn)
            {
                printfd.ColumnGap = 0;
                printfd.ColumnWidth = pd.PrintableAreaWidth;
            }

            DocumentPaginator printpaginator = ((IDocumentPaginatorSource)printfd).DocumentPaginator;
            printpaginator = new PageRangeDocumentPaginator(printpaginator, (rbPages.IsChecked == true ? tbPages.Text : ""));
            pd.PrintDocument(printpaginator, description);
        }

        private void tbPages_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9-]+").IsMatch(e.Text);
        }

        private void cmboPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetDuplexing();
            PreparePreview();
        }

        private void cmboOrientation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreparePreview();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PreparePrint();

            DialogResult = true;
            Close();
        }
    }
}
