using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Model.OrderModel
{
    public class QuoteOrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string QuoteCode { get; set; }
        public string Mode { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
