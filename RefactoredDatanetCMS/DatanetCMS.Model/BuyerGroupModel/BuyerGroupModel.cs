using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Model.BuyerGroupModel
{
   public partial class BuyerGroupModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
    public class PageBuyerGroupModel
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<BuyerGroupModel> Items { get; set; }

    }

    public class GroupProductModel
    {
        public GroupProductModel()
        {
            CustomerTempModelList=new List<CustomerTempModel>();
            ProductTempModelList=new List<ProductTempModel>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public List<CustomerTempModel> CustomerTempModelList { get; set; }
        public List<ProductTempModel> ProductTempModelList { get; set; }
    }

    public class CustomerTempModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductTempModel
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public decimal? Price { get; set; }
    }
}
