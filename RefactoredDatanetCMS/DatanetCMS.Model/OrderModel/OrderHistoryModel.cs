using System;
using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public class OrderHistoryModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public string Note { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateBy { get; set; }
    }
}
