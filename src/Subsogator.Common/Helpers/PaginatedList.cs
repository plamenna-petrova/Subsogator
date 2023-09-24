using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Subsogator.Common.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        [Display(Name = "Results Per Page")]
        public PageResultsEnum PageResults { get; set; }

        public static PaginatedList<T> Create(
            IEnumerable<T> source,
            int pageIndex,
            int pageSize
        )
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public T ViewModel { get; }
    }
}
