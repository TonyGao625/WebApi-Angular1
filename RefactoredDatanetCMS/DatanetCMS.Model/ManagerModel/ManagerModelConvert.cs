using DatanetCMS.DAO;

namespace DatanetCMS.Model.ManagerModel
{
    public static class ManagerModelConvert
    {
        public static Manager ToManagerModel(this ManagerModel model)
        {
            var manager = new DAO.Manager()
            {
                Id = model.Id,
                Name = model.Name,
                LoginName = model.LoginName,
                Password = model.Password,
                Role = model.Role,
                Email = model.Email,
                Phone = model.Phone
            };
            return manager;
        }

        public static ManagerModel ToManagerModel(this Manager manager)
        {
            var model = new ManagerModel()
            {
                Id = manager.Id,
                Name = manager.Name,
                LoginName = manager.LoginName,
                Password = manager.Password,
                Role = manager.Role,
                Email = manager.Email,
                Phone = manager.Phone
            };
            return model;
        }
    }
}
