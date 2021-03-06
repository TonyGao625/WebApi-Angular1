//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatanetCMS.DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.ContractNumbers = new HashSet<ContractNumber>();
            this.DeliveryAddresses = new HashSet<DeliveryAddress>();
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string LoginId { get; set; }
        public string LoginPassword { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string ContractNo { get; set; }
        public string PurchaseType { get; set; }
        public Nullable<bool> DisplayContractNo { get; set; }
        public Nullable<bool> PoDocMandatory { get; set; }
        public Nullable<int> BuyerGroupId { get; set; }
        public Nullable<decimal> DeliveryCharge { get; set; }
        public string Logo { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public Nullable<System.DateTime> ExpireTime { get; set; }
    
        public virtual BuyerGroup BuyerGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractNumber> ContractNumbers { get; set; }
        public virtual Manager Manager { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
