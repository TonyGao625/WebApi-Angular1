using System;

namespace DatanetCMS.Model.OrderModel
{
    public class CustomerOrderModel
    {
        public int Id { get; set; }
        public string QuoteCode { get; set; }
        public string OrderCode { get; set; }
        public string ManagerName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreateTime { get; set; }

    }
}
