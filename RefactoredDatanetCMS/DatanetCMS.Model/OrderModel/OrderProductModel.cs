namespace DatanetCMS.Model.OrderModel
{
    public partial class OrderProductModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }

    public partial class OrderProductModel
    {
        public ProductModel.ProductModel ProductModel{ get; set; }
    }
}
