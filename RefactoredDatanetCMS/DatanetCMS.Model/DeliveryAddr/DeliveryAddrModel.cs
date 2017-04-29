using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Model.DeliveryAddr
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
