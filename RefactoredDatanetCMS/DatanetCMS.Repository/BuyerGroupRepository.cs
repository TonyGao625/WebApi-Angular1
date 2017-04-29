using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
   public  class BuyerGroupRepository
    {
        public async Task<List<BuyerGroup>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.BuyerGroups.Where(x => x.ExpireTime == null).OrderBy(x => x.Code).ToListAsync();
            }
        }

       public async Task<List<GroupProduct>> GetGroupProductByBuyerGroupId(int buyerGroupId)
       {
           using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
           {
                return await context.GroupProducts.Where(x => x.ExpireTime == null&&x.BuyerGroupId==buyerGroupId).ToListAsync();
            }
       }

       public async Task<BuyerGroup> GetGroupByName(string code, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.BuyerGroups.Where(x => x.Code == code && x.Id != id&&x.ExpireTime==null).FirstOrDefaultAsync();
            }
        }
        public async Task<BuyerGroup> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.BuyerGroups.Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<int> AddOrUpdate(BuyerGroup buyerGroup)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.BuyerGroups.AddOrUpdate(buyerGroup);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<int> AddOrUpdateGroupProduct(GroupProduct groupProduct)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.GroupProducts.AddOrUpdate(groupProduct);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<int> DeleteGroupProduct(GroupProduct groupProduct)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                groupProduct.ExpireTime=DateTime.UtcNow;
                context.GroupProducts.AddOrUpdate(groupProduct);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<int> DeleteBuyerGroup(BuyerGroup buyerGroup)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                buyerGroup.ExpireTime = DateTime.UtcNow;
                context.BuyerGroups.AddOrUpdate(buyerGroup);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<GroupProduct> GetGroupPpoductById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.GroupProducts.Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

    }
}
