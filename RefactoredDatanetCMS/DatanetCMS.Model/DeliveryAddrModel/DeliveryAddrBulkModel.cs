using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Model.DeliveryAddrModel
{
    public class DeliveryAddrBulkModel
    {
        public DeliveryAddrBulkModel() {
            DeliveryAddrModel = new List<Model.DeliveryAddrModel.DeliveryAddrModel>();
        }
        public List<DeliveryAddrModel> DeliveryAddrModel { get; set; }
    }
}
