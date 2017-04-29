using DatanetCMS.DAO;

namespace DatanetCMS.Model.UomModel
{
    public static class UomModelConvert
    {
        public static UomModel ToUomModel(this Uom uom)
        {
            var model = new UomModel()
            {
                Id = uom.Id,
                Name = uom.Name
            };
            return model;
        }

        public static Uom ToUomModel(this UomModel uom)
        {
            var model = new Uom()
            {
                Id = uom.Id,
                Name = uom.Name
            };
            return model;
        }
    }
}
