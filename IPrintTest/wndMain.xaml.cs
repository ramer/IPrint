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
            IPrintProvider.ShowPreview(fdSample);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            IPrintProvider.PrintDocument(fdSample);
        }
    }
}
