using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class UomRepository
    {
        public UomRepository()
        {
        }

        /// <summary>
        /// get all uom
        /// </summary>
        /// <returns></returns>
        public async Task<List<Uom>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Uoms.Where(x=>x.ExpireTime == null).OrderBy(c => c.Name).ToListAsync();
            }
        }

        /// <summary>
        /// get uom by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Uom> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Uoms.Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// add or update uom
        /// </summary>
        /// <param name="uom"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(Uom uom)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Uoms.AddOrUpdate(uom);
                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// soft delete uom
        /// </summary>
        /// <param name="uom"></param>
        /// <returns></returns>
        public async Task<int> Delete(Uom uom)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                uom.ExpireTime = DateTime.UtcNow;
                context.Uoms.AddOrUpdate(uom);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<Uom> GetUomByName(string name, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Uoms.Where(x=>x.Name == name && x.Id != id&&x.ExpireTime==null).FirstOrDefaultAsync();
            }
        }
    }
}
