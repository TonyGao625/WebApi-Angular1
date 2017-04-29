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
    
    public partial class BuyerGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuyerGroup()
        {
            this.Customers = new HashSet<Customer>();
            this.GroupProducts = new HashSet<GroupProduct>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public Nullable<System.DateTime> ExpireTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupProduct> GroupProducts { get; set; }
    }
}