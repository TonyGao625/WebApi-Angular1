using DatanetCMS.DAO;

namespace DatanetCMS.Model.ContractNumberModel
{
    public static class ContractNumberModelConvert
    {
        public static ContractNumberModel ToContractNumberModel(this ContractNumber cn)
        {
            var model = new ContractNumberModel()
            {
                Id = cn.Id,
                CustomerId = cn.CustomerId,
                Desc = cn.Desc,
                Name = cn.Name
            };
            return model;
        }

        public static ContractNumber ToContractNumberModel(this ContractNumberModel model)
        {
            var cn = new ContractNumber()
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                Desc = model.Desc,
                Name = model.Name
            };
            return cn;
        }
    }
}
