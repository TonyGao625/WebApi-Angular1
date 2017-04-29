
using System.Collections.Generic;

namespace DatanetCMS.Model.UomModel
{
    public partial class UomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class PageUomModel
    {
        public  int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<UomModel> Items { get; set; }

    }
}
