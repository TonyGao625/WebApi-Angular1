namespace DatanetCMS.Model.OrderModel
{
    public class OrderNumberModel
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public int CurrentNumber { get; set; }
        public string Mode { get; set; }
    }
}
