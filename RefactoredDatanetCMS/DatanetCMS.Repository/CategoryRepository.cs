using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class CategoryRepository
    {
        public async Task<List<Category>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Categories.Where(x => x.ExpireTime == null).OrderBy(c => c.CategoryName).ToListAsync();
            }
        }

        public async Task<Category> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Categories.Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<int> Delete(Category category)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                category.ExpireTime = DateTime.UtcNow;
                context.Categories.AddOrUpdate(category);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategoryByName(string name, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Categories.Where(x => x.CategoryName == name && x.Id != id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<int> AddOrUpdate(Category category)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Categories.AddOrUpdate(category);
                return await context.SaveChangesAsync();
            }
        }
    }
}
