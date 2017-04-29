using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class OrderAddrRepository
    {
        public OrderAddrRepository()
        {

        }

        /// <summary>
        /// add or update order addr
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(OrderAddress addr)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.OrderAddresses.AddOrUpdate(addr);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<List<OrderAddress>> GetOrderAddrByCustomerId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.OrderAddresses.Where(x=>x.Order.CustomerId == id).ToListAsync();
            }
        }

        public async Task<OrderAddress> GetOrderAddrByOrderId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.OrderAddresses.Where(x => x.OrderId==id).FirstOrDefaultAsync();
            }
        }
    }
}
