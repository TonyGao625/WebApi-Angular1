using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderHistoryModelConvert
    {
        public static OrderHistoryModel ToOrderHistoryModel(this OrderHistory history)
        {
            var model = new OrderHistoryModel()
            {
                Id = history.Id,
                CreateBy = history.CreateBy,
                CreateTime = history.CreateTime,
                FieldName = history.FieldName,
                NewValue = history.NewValue,
                Note = history.Note,
                OldValue = history.OldValue,
                Operator = history.Operator,
                OrderId = history.OrderId,
                TableName = history.TableName
            };
            return model;
        }
        public static OrderHistory ToOrderHistoryModel(this OrderHistoryModel history)
        {
            var model = new OrderHistory()
            {
                Id = history.Id,
                CreateBy = history.CreateBy,
                CreateTime = history.CreateTime,
                FieldName = history.FieldName,
                NewValue = history.NewValue,
                Note = history.Note,
                OldValue = history.OldValue,
                Operator = history.Operator,
                OrderId = history.OrderId,
                TableName = history.TableName
            };
            return model;
        }
    }
}
