using System.Collections.Generic;

namespace DatanetCMS.Model
{
    public class PageModel<T>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
    public class PageSingleModel<T>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public T Item { get; set; }
    }
}
