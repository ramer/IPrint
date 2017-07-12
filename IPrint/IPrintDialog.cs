using System;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

using System.Xml;

namespace IPrint
{
    public class IPrintDialog
    {

        public static FlowDocument FlowDocumentClone(FlowDocument fd)
        {
            string str = XamlWriter.Save(fd);
            StringReader stringReader = new StringReader(str);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (FlowDocument)XamlReader.Load(xmlReader);
        }

        public static UIElement UIElementClone(UIElement uie)
        {
            string str = XamlWriter.Save(uie);
            StringReader stringReader = new StringReader(str);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (UIElement)XamlReader.Load(xmlReader);
        }

        public static bool PreviewDocument(FlowDocument flowdocument, string description = "", bool singlecolumn = true, Window owner = null)
        {
            if (flowdocument == null) { return false; }

            FlowDocumentPreview w = new FlowDocumentPreview();
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
                    fd.PageWidth = area.ExtentWidth + area.OriginWidth * 2;
                    fd.PageHeight = area.ExtentHeight + area.OriginHeight * 2;
                }
                else if (pd.PrintTicket.PageOrientation == PageOrientation.Landscape)
                {
                    fd.PageWidth = area.ExtentHeight + area.OriginHeight * 2;
                    fd.PageHeight = area.ExtentWidth + area.OriginWidth * 2;
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

        public static bool PreviewUIElement(UIElement uielement, string description = "", Window owner = null)
        {
            if (uielement == null) { return false; }

            UIElementPreview w = new UIElementPreview();
            w.uie = UIElementClone(uielement);
            w.description = description;

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

        public static bool PrintUIElement(UIElement uielement, string description = "")
        {
            if (uielement == null) { return false; }

            UIElement uie = UIElementClone(uielement);
            PrintDialog pd = new PrintDialog();
            Border container = new Border();

            PageImageableArea area = pd.PrintQueue.GetPrintCapabilities().PageImageableArea;

            if (area != null && area != null)
            {
                if (pd.PrintTicket.PageOrientation == PageOrientation.Portrait)
                {
                    container.Width = area.ExtentWidth + area.OriginWidth * 2;
                    container.Height = area.ExtentHeight + area.OriginHeight * 2;
                }
                else if (pd.PrintTicket.PageOrientation == PageOrientation.Landscape)
                {
                    container.Width = area.ExtentHeight + area.OriginHeight * 2;
                    container.Height = area.ExtentWidth + area.OriginWidth * 2;
                }
            }

            container.Child = uie;
            container.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            container.Arrange(new Rect(container.DesiredSize));
            container.UpdateLayout();

            pd.PrintVisual(uie, description);
            return true;
        }

    }
}
