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
    
    public partial class GroupCustomer
    {
        public int Id { get; set; }
        public Nullable<int> BuyerGroupId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> ExpireTime { get; set; }
    
        public virtual BuyerGroup BuyerGroup { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
