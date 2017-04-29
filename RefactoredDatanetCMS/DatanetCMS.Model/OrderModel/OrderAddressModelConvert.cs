using DatanetCMS.DAO;

namespace DatanetCMS.Model.OrderModel
{
    public static class OrderAddressModelConvert
    {
        public static OrderAddressModel ToOrderAddressModel(this OrderAddress addr)
        {
            var model = new OrderAddressModel()
            {
                Id = addr.Id,
                Addr1 = addr.Addr1,
                Addr2 = addr.Addr2,
                Addr3 = addr.Addr3,
                AddrId = addr.AddrId,
                Phone = addr.Phone,
                OrderId = addr.OrderId,
                PostCode = addr.PostCode,
                State = addr.State
            };
            return model;
        }
        public static OrderAddress ToOrderAddressModel(this OrderAddressModel addr)
        {
            var model = new OrderAddress()
            {
                Id = addr.Id,
                Addr1 = addr.Addr1,
                Addr2 = addr.Addr2,
                Addr3 = addr.Addr3,
                AddrId = addr.AddrId,
                Phone = addr.Phone,
                OrderId = addr.OrderId,
                PostCode = addr.PostCode,
                State = addr.State
            };
            return model;
        }
    }
}
