using System.Windows;
using IPrint;

namespace IPrintTest
{
    public partial class wndMain : Window
    {
        public wndMain()
        {
            InitializeComponent();
        }

        private void btnFlowDocumentPreview_Click(object sender, RoutedEventArgs e)
        {
            IPrintDialog.PreviewDocument(fdSample);
        }

        private void btnFlowDocumentPrint_Click(object sender, RoutedEventArgs e)
        {
            IPrintDialog.PrintDocument(fdSample);
        }

        private void btnUIElementPreview_Click(object sender, RoutedEventArgs e)
        {
            IPrintDialog.PreviewUIElement(grdSample);
        }

        private void btnUIElementPrint_Click(object sender, RoutedEventArgs e)
        {
            IPrintDialog.PrintUIElement(grdSample);
        }
    }
}
