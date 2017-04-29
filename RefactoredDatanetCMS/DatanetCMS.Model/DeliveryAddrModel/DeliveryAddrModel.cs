namespace DatanetCMS.Model.DeliveryAddrModel
{
    public class DeliveryAddrModel
    {
        public int Id { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public int? CustomerId { get; set; }
        
    }
}
