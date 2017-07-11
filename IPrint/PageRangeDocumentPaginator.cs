using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace IPrint
{
    /// <summary>
    /// Encapsulates a DocumentPaginator and allows
    /// to paginate just some specific pages (a "PageRange")
    /// of the encapsulated DocumentPaginator
    /// (c) Thomas Claudius Huber 2010 
    /// http://www.thomasclaudiushuber.com
    /// </summary>
    public class PageRangeDocumentPaginator : DocumentPaginator
    {
        private int _startIndex;
        private int _endIndex;
        private DocumentPaginator _paginator;
        public PageRangeDocumentPaginator(
          DocumentPaginator paginator,
          string pagerange)
        {
            _paginator = paginator;
            _paginator.ComputePageCount();

            string range = pagerange.Replace(" ", "");
            string[] ranges = range.Split(new char[] { '-' }, 2, System.StringSplitOptions.RemoveEmptyEntries);
            int from; int to;
            if (!((ranges.Count() >= 1) && (int.TryParse(ranges[0], out from)) && (from > 0))) { from = 0; }
            if (!((ranges.Count() >= 2) && (int.TryParse(ranges[1], out to)) && (to > from))) { to = 0; }

            if (from > 0 & to > 0)
            {
                _startIndex = Math.Min(from - 1, _paginator.PageCount - 1);
                _endIndex = Math.Min(to - 1, _paginator.PageCount - 1);
            }
            else if (from > 0 & to == 0)
            {
                _startIndex = Math.Min(from - 1, _paginator.PageCount - 1);
                _endIndex = Math.Min(from - 1, _paginator.PageCount - 1);
            }
            else
            {
                _startIndex = 0;
                _endIndex = _paginator.PageCount - 1;
            }
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            return _paginator.GetPage(pageNumber + _startIndex);
        }

        public override bool IsPageCountValid
        {
            get { return true; }
        }

        public override int PageCount
        {
            get
            {
                if (_startIndex > _paginator.PageCount - 1)
                    return 0;
                if (_startIndex > _endIndex)
                    return 0;

                return _endIndex - _startIndex + 1;
            }
        }

        public override Size PageSize
        {
            get { return _paginator.PageSize; }
            set { _paginator.PageSize = value; }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return _paginator.Source; }
        }
    }
}
