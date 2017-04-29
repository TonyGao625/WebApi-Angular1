using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class ManagerRepository
    {
        public ManagerRepository()
        {
        }

        /// <summary>
        /// get all manager
        /// </summary>
        /// <returns></returns>
        public async Task<List<Manager>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Managers.Where(x => x.ExpireTime == null).OrderBy(x=>x.Name).ToListAsync();
            }
        }

        public async Task<List<Manager>> GetAllAdmin()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Managers.Where(x => x.ExpireTime == null&&x.Role=="admin").OrderBy(x => x.Name).ToListAsync();
            }
        }
        /// <summary>
        /// get manager by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Manager> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Managers.Where(x => x.ExpireTime == null && x.Id == id).FirstOrDefaultAsync();
            }
        }
        /// <summary>
        /// save or update manager
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(Manager manager)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Managers.AddOrUpdate(manager);
                return await context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// delete manager by id
        /// soft delete
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<int> Delete(Manager manager)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                manager.ExpireTime = DateTime.UtcNow;
                context.Managers.AddOrUpdate(manager);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<Manager> GetManagerByName(string name, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {

                return await context.Managers.Where(x => x.LoginName == name && x.Id != id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }


        public async Task<Manager> GetManagerByManagerName(string name, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {

                return await context.Managers.Where(x => x.Name == name && x.Id != id&&x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }
        public async Task<Manager> GetByName(string name)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Managers.Where(x => x.ExpireTime == null && x.LoginName == name).FirstOrDefaultAsync();
            }
        }

        public async Task<Manager> GetAdmin()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Managers.Where(x => x.ExpireTime == null && x.Role == "Admin").FirstOrDefaultAsync();
            }
        }
    }
}
