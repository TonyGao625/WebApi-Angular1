using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public static class QuoteOrderModelConvert
    {
        public static QuoteOrderModel ToQuoteOrderModel(this Order order)
        {
            var model = new QuoteOrderModel
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                QuoteCode = order.QuoteCode,
                Mode = order.Mode,
                CompanyName = order.CompanyName,
                ContactName = order.ContactName,
                Amount = order.Amount,
                CreateTime = order.CreateTime
            };
            return model;
        }
    }
}
