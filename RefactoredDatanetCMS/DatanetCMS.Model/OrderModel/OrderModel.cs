using System;
using System.Collections.Generic;

namespace DatanetCMS.Model.OrderModel
{
    public partial class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string QuoteCode { get; set; }
        public int? CustomerId { get; set; }
        public string ContactEmail { get; set; }
        public string Mode { get; set; }
        public string PurchaseType { get; set; }
        public string PurchaseNumber { get; set; }
        public bool?  IsBuildOrder { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public double? GSTRate { get; set; }
        public decimal? GST { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreateTime { get; set; }

        public string CompanyName { get; set; }
        public string CustomerContactName { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string PoDocPath { get; set; }
    }

    public partial class OrderModel
    {
        public List<OrderProductModel> OrderProductModels{ get; set; }

        public List<OrderAddressModel> OrderAddressModels{ get; set; }

        public CustomerModel.CustomerModel CustomerModel { get; set; }
    }
}
