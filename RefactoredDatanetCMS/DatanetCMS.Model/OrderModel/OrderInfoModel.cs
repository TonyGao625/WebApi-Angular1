using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DatanetCMS.Model.OrderModel
{
    public class OrderInfoModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string PurchaseCode { get; set; }
        public string Mode { get; set; }
        public double GSTRate { get; set; }
        public decimal GST { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Amount { get; set; }
        public string PDFPath { get; set; }
        public List<OrderDevliveryAddrModel> OrderAddres { get; set; }
        public List<OrderDocModel> OrderDocs { get; set; }
        public List<OrderProductModel> OrderProducts { get; set; }
        public int TimeZone { get; set; }
    }
}
