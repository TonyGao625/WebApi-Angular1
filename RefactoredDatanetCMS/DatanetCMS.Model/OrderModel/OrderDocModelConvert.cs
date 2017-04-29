using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderDocModelConvert
    {
        public static OrderDocModel ToOrderDocModel(this OrderDoc doc)
        {
            var model = new OrderDocModel()
            {
                Id = doc.Id,
                OrderId = doc.OrderId,
                PoDocPath = doc.PoDocPath
            };
            return model;
        }
        public static OrderDoc ToOrderDocModel(this OrderDocModel doc)
        {
            var model = new OrderDoc()
            {
                Id = doc.Id,
                OrderId = doc.OrderId,
                PoDocPath = doc.PoDocPath
            };
            return model;
        }
    }
}
