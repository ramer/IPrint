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

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            IPrintProvider.ShowPreview(fdSample, "FlowDocument print test");
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            IPrintProvider.PrintDocument(fdSample, "FlowDocument print test");
        }
    }
}
