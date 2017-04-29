using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using DatanetCMS.DAO;
using System.Linq;
using System.Data.Entity;

namespace DatanetCMS.Repository
{
    public class OrderDocRepository
    {
        public OrderDocRepository()
        {

        }

        /// <summary>
        /// add or update order doc
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(OrderDoc doc)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.OrderDocs.AddOrUpdate(doc);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<OrderDoc> GetOrderDocByOrderId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.OrderDocs.Where(x => x.OrderId == id).FirstOrDefaultAsync();
            }
        }
    }
}
