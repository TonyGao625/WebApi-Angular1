namespace DatanetCMS.Model.Manager
{
    public static class ManagerModelConvert
    {
        public static DAO.Manager ToManagerModel(this ManagerModel model)
        {
            var manager = new DAO.Manager()
            {
                Id = model.Id,
                Name = model.Name
            };
            return manager;
        }

        public static ManagerModel ToManagerModel(this DAO.Manager manager)
        {
            var model = new ManagerModel()
            {
                Id = manager.Id,
                Name = manager.Name
            };
            return model;
        }
    }
}
