
using System.Collections.Generic;

namespace DatanetCMS.Model.ProductModel
{
    public partial class ProductModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string Vendor { get; set; }
        public int? UomId { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
    }

    public partial class ProductModel
    {
        public UomModel.UomModel Uom { get; set; }
        public CategoryModel.CategoryModel Category { get; set; }
    }

    public class PageProductModel
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<ProductModel> Items { get; set; }
    }
}
