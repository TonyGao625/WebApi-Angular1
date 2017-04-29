using System.Linq;
using DatanetCMS.DAO;
using DatanetCMS.Model.CustomerModel;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderModelConvert
    {
        public static OrderModel ToOrderModel(this Order order)
        {
            var model = new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ContactEmail = order.ContactEmail,
                Mode = order.Mode,
                OrderCode = order.OrderCode,
                QuoteCode = order.QuoteCode,
                PurchaseNumber = order.PurchaseNumber,
                PurchaseType = order.PurchaseType,
                IsBuildOrder = order.IsBuildOrder,
                ContactPhone = order.ContactPhone,
                ContactName = order.ContactName,
                GST = order.GST,
                GSTRate = order.GSTRate,
                DeliveryCharge = order.DeliveryCharge,
                Amount = order.Amount,
                CreateTime = order.CreateTime,
                CompanyName = order.CompanyName,
                CustomerContactName = order.CustomerContactName,
                ManagerId = order.ManagerId,
                ManagerName = order.ManagerName,
            };
            if(order.OrderProducts != null)
            {
                model.OrderProductModels = order.OrderProducts.Select(x => x.ToOrderProductModel()).ToList();
            }
            if(order.OrderAddresses != null)
            {
                model.OrderAddressModels = order.OrderAddresses.Select(x => x.ToOrderAddressModel()).ToList();
            }
            return model;
        }

        public static Order ToOrderModel(this OrderModel order)
        {
            var model = new Order()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ContactEmail = order.ContactEmail,
                Mode = order.Mode,
                OrderCode = order.OrderCode,
                QuoteCode = order.QuoteCode,
                PurchaseNumber = order.PurchaseNumber,
                PurchaseType = order.PurchaseType,
                ContactName = order.ContactName,
                ContactPhone = order.ContactPhone,
                GST = order.GST,
                GSTRate = order.GSTRate,
                DeliveryCharge = order.DeliveryCharge,
                Amount = order.Amount,
                CreateTime = order.CreateTime,
            };
            return model;
        }

        public static OrderModel ToQuoteModel(this Order order)
        {
            var model = new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ContactEmail = order.ContactEmail,
                Mode = order.Mode,
                OrderCode = order.OrderCode,
                QuoteCode = order.QuoteCode,
                PurchaseNumber = order.PurchaseNumber,
                PurchaseType = order.PurchaseType,
                IsBuildOrder = order.IsBuildOrder,
                ContactPhone = order.ContactPhone,
                ContactName = order.ContactName,
                GST = order.GST,
                GSTRate = order.GSTRate,
                DeliveryCharge = order.DeliveryCharge,
                Amount = order.Amount,
                CreateTime = order.CreateTime,

                CompanyName = order.CompanyName,
                CustomerContactName = order.CustomerContactName,
                ManagerId = order.ManagerId,
                ManagerName = order.ManagerName,
            };
            if (order.OrderProducts != null)
            {
                model.OrderProductModels = order.OrderProducts.Select(x => x.ToQuoteOrderProductModel()).ToList();
            }
            return model;
        }

        public static CustomerOrderModel ToCustomerOrderModel(this Order order)
        {
            var model = new CustomerOrderModel()
            {
                Id = order.Id,
                QuoteCode = order.QuoteCode,
                OrderCode = order.OrderCode,
                ManagerName = order.ManagerName,
                Amount = order.Amount,
                CreateTime = order.CreateTime,
            };
            return model;
        }
    }
}
