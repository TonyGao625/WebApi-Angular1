using DatanetCMS.DAO;

namespace DatanetCMS.Model.DeliveryAddrModel
{
    public static class DeliveryAddrModelConvert
    {
        public static DeliveryAddrModel ToDeliveryAddrModel(this DeliveryAddress addr)
        {
            var model = new DeliveryAddrModel
            {
                Id = addr.Id,
                Addr1 = addr.Addr1,
                Addr2 = addr.Addr2,
                Addr3 = addr.Addr3,
                CustomerId = addr.CustomerId,
                PostCode = addr.PostCode,
                Phone = addr.Phone,
                State = addr.State
            };
            return model;
        }

        public static DeliveryAddress ToDeliveryAddrModel(this DeliveryAddrModel model)
        {
            var addr = new DeliveryAddress
            {
                Id = model.Id,
                Addr1 = model.Addr1,
                Addr2 = model.Addr2,
                Addr3 = model.Addr3,
                PostCode = model.PostCode,
                Phone = model.Phone,
                State = model.State,
                CustomerId = model.CustomerId
            };
            return addr;
        }
    }
}
