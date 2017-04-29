using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Model.CategoryModel
{
    public partial class CategoryModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class PageCategoryModel
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<CategoryModel> Items { get; set; }

    }
}
