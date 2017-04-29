using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class OrderHistoryRepository
    {
        public OrderHistoryRepository()
        {
            
        }

        /// <summary>
        /// add or update order history
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(OrderHistory history)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.OrderHistories.AddOrUpdate(history);
                return await context.SaveChangesAsync();
            }
        }
    }
}
