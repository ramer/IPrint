using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace IPrint
{
    public class IPrintProvider
    {

        public static bool ShowPreview (FlowDocument flowdocument, string description = "", bool singlecolumn = true, Window owner = null)
        {
            if (flowdocument == null) { return false; }

            Preview w = new Preview();
            w.fd = FlowDocumentClone(flowdocument);
            w.description = description;
            w.singlecolumn = singlecolumn;

            if (!(owner == null)) { w.Owner = owner; w.Icon = owner.Icon; }

            bool? dialogResult = w.ShowDialog();
            if (dialogResult == null)
            {
                return false;
            }
            else if (dialogResult == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool PrintDocument(FlowDocument flowdocument, string description = "", bool singlecolumn = true)
        {
            if (flowdocument == null) { return false; }

            FlowDocument fd = FlowDocumentClone(flowdocument);
            PrintDialog pd = new PrintDialog();

            PageImageableArea area = pd.PrintQueue.GetPrintCapabilities().PageImageableArea;
            
            if (area != null && area != null)
            {
                if (pd.PrintTicket.PageOrientation == PageOrientation.Portrait)
                {
                    fd.PageWidth = area.ExtentWidth + area.OriginWidth;
                    fd.PageHeight = area.ExtentHeight + area.OriginHeight;
                }
                else if (pd.PrintTicket.PageOrientation == PageOrientation.Landscape)
                {
                    fd.PageWidth = area.ExtentHeight + area.OriginHeight;
                    fd.PageHeight = area.ExtentWidth + area.OriginWidth;
                }
            }

            if (singlecolumn)
            {
                fd.ColumnGap = 0;
                fd.ColumnWidth = pd.PrintableAreaWidth;
            }

            DocumentPaginator paginator = ((IDocumentPaginatorSource)fd).DocumentPaginator;
            pd.PrintDocument(paginator, description);
            return true;
        }

        public static FlowDocument FlowDocumentClone(FlowDocument fd)
        {
            string str = XamlWriter.Save(fd);
            StringReader stringReader = new StringReader(str);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (FlowDocument)XamlReader.Load(xmlReader);
        }

    }
}
