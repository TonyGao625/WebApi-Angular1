using DatanetCMS.Model.OrderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderDeliveryAddrModelConvert
    {
        public static OrderDevliveryAddrModel ToOrderDevliveryAddrModel(this DeliveryAddress addr)
        {
            var model = new OrderDevliveryAddrModel()
            {
                Id = addr.Id,
                Addr1 = addr.Addr1,
                Addr2 = addr.Addr2,
                Addr3 = addr.Addr3,
                CustomerId = addr.CustomerId,
                PostCode = addr.PostCode,
                Phone = addr.Phone,
                State = addr.State,
                Count = 0
            };
            return model;
        }
    }
}
