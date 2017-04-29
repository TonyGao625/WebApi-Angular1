using DatanetCMS.DAO;

namespace DatanetCMS.Model.BuyerGroupModel
{
    public static class BuyerGroupModelConvert
    {
        public static BuyerGroupModel ToBuyerGroupModel(this BuyerGroup buyerGroup)
        {
            var model = new BuyerGroupModel()
            {
                Id = buyerGroup.Id,
                Code = buyerGroup.Code
            };
            return model;
        }

    }
}
