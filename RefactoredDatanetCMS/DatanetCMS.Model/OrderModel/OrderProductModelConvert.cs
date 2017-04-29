using DatanetCMS.DAO;
using DatanetCMS.Model.ProductModel;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderProductModelConvert
    {
        public static OrderProductModel ToOrderProductModel(this OrderProduct product)
        {
            var model = new OrderProductModel
            {
                Id = product.Id,
                OrderId = product.OrderId,
                Price = product.Price,
                ProductId = product.ProductId,
                Quantity = product.Quantity,
            };
            if (product.Product != null)
            {
                model.ProductModel = product.Product.ToProductModelWithoutImage();
            }
            return model;
        }
        public static OrderProduct ToOrderProductModel(this OrderProductModel product)
        {
            var model = new OrderProduct()
            {
                Id = product.Id,
                OrderId = product.OrderId,
                Price = product.Price,
                ProductId = product.ProductId,
                Quantity = product.Quantity
            };
            return model;
        }

        public static OrderProductModel ToQuoteOrderProductModel(this OrderProduct product)
        {
            var model = new OrderProductModel
            {
                Id = product.Id,
                OrderId = product.OrderId,
                Price = product.Price,
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                //ProductModel = product.Product?.ToProductModel()
            };
            if (product.Product != null)
            {
                model.ProductModel = product.Product.ToQuoteProductModel();
            }
            return model;
        }

        public static OrderProductModel ToOrderProductModelWithImage(this OrderProduct product)
        {
            var model = new OrderProductModel
            {
                Id = product.Id,
                OrderId = product.OrderId,
                Price = product.Price,
                ProductId = product.ProductId,
                Quantity = product.Quantity,
            };
            if (product.Product != null)
            {
                model.ProductModel = product.Product.ToProductModelWithImage();
            }
            return model;
        }
    }
}
