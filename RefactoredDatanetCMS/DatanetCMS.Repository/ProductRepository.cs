using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class ProductRepository
    {
        public ProductRepository()
        {
            
        }

        public async Task<List<Product>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Products
                    .Include(a => a.Uom)
                    .Include(a=>a.Category)
                    .Where(x=>x.ExpireTime==null).OrderBy(x=>x.Code).ToListAsync();
            }
        }

        public async Task<Product> GetProductByName(string code, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Products.Where(x => x.Code == code && x.Id != id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<Product> GetById(int? id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Products.Include(x=>x.Uom).Include(x=>x.Category).Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<Product> GetByIdAndCategoryId(int? id, int? categoryId)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Products.Include(x => x.Uom).Where(x => x.Id == id && x.ExpireTime == null&&x.CategoryId==categoryId).FirstOrDefaultAsync();
            }
        }

        public async Task<int> AddOrUpdate(Product product)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Products.AddOrUpdate(product);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> Delete(Product product)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                product.ExpireTime=DateTime.UtcNow;
                context.Products.AddOrUpdate(product);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteRange(List<Product> products)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Products.RemoveRange(products);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductByUomId(int uomId)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Products.Where(x => x.UomId == uomId && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<Product> GetProductByCategoryId(int categoryId)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Products.Where(x => x.CategoryId == categoryId && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }
    }
}
