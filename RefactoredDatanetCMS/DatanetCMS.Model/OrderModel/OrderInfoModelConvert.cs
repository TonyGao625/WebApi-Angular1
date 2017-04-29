using System.Collections.Generic;
using System.Linq;
using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderInfoModelConvert
    {
        public static OrderModel ToOrderModel(this OrderInfoModel model)
        {
            var orderModel = new OrderModel()
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                ContactEmail = model.ContactEmail,
                ContactName = model.ContactName,
                ContactPhone = model.ContactPhone,
                GSTRate = model.GSTRate,
                GST = model.GST,
                DeliveryCharge = model.DeliveryCharge,
                Amount = model.Amount
            };
            return orderModel;
        }
        public static OrderAddress ToOrderAddrModel(this OrderInfoModel model)
        {
            var orderAddrModel = model.OrderAddres.FirstOrDefault();
            if (orderAddrModel != null)
            {
                var orderAddr = new OrderAddress
                {
                    Addr1 = orderAddrModel.Addr1,
                    Addr2 = orderAddrModel.Addr2,
                    Addr3 = orderAddrModel.Addr3,
                    AddrId = orderAddrModel.Id,
                    PostCode = orderAddrModel.PostCode,
                    State = orderAddrModel.State,
                    Phone = orderAddrModel.Phone
                };
                return orderAddr;
            }
            return null;
        }
        public static OrderDoc ToOrderDocModel(this OrderInfoModel model)
        {
            var orderDocModel = model.OrderDocs.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(orderDocModel?.PoDocPath))
            {
                var orderDoc = new OrderDoc
                {
                    PoDocPath = orderDocModel.PoDocPath
                };
                return orderDoc;
            }
            return null;
        }
        public static List<OrderProduct> ToOrderProductsModel(this OrderInfoModel model)
        {
            var orderProductModel = model.OrderProducts;
            if (orderProductModel != null && orderProductModel.Count > 0)
            {
                var orderProducts = new List<OrderProduct>();
                foreach (var productModel in orderProductModel)
                {
                    var product = new OrderProduct()
                    {
                        Price = productModel.Price,
                        ProductId = productModel.ProductId,
                        Quantity = productModel.Quantity,
                    };
                    orderProducts.Add(product);
                }
                return orderProducts;
            }
            return null;
        }
    }
}
