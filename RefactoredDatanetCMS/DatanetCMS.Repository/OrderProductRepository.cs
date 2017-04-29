using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Repository
{
    public class OrderProductRepository
    {
        public OrderProductRepository()
        {

        }

        /// <summary>
        /// add or update order product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(OrderProduct product)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.OrderProducts.AddOrUpdate(product);
                return await context.SaveChangesAsync();
            }
        }
    }
}
