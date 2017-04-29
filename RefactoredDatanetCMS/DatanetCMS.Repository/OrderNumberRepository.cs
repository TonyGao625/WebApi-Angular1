using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class OrderNumberRepository
    {
        public OrderNumberRepository()
        { }

        public async Task<OrderNumber> GetOrderNumberByMode(string mode)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.OrderNumbers.Where(x=>x.Mode == mode).FirstOrDefaultAsync();
            }
        }

        public async Task<int> AddOrUpdate(OrderNumber orderNumber)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.OrderNumbers.AddOrUpdate(orderNumber);
                return await context.SaveChangesAsync();
            }
        }

    }
}
