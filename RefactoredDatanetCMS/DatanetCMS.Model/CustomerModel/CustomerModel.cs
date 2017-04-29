namespace DatanetCMS.Model.CustomerModel
{
    public partial class CustomerModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string LoginId { get; set; }
        public string LoginPassword { get; set; }
        public string ContactName { get; set; }
        //public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int? ManagerId { get; set; }
        public string ContractNo { get; set; }
        public bool? DisplayContractNo { get; set; }
        //public bool? PurchaseOnContract { get; set; }
        //public bool?  PoMandatory { get; set; }
        public bool? PoDocMandatory { get; set; }
        public int? BuyerGroupId { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public string Logo { get; set; }
        public string PurchaseType { get; set; }
    }

    public partial class CustomerModel
    {
        public ManagerModel.ManagerModel Manager { get; set; }
        public BuyerGroupModel.BuyerGroupModel BuyerGroup { get; set; }

    }
}
