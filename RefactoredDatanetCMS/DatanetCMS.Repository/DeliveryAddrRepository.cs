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
    public class DeliveryAddressRepository
    {
        public DeliveryAddressRepository()
        {
        }

        /// <summary>
        /// get all addr
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryAddress>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.DeliveryAddresses.Where(x => x.ExpireTime == null).ToListAsync();
            }
        }

        /// <summary>
        /// get addresses by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<DeliveryAddress>> GetAddrsByCustomerId(int customerId)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.DeliveryAddresses.Where(x => x.ExpireTime == null && x.CustomerId == customerId).ToListAsync();
            }
        }

        /// <summary>
        /// get addr by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeliveryAddress> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.DeliveryAddresses.Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// add or update addr
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(DeliveryAddress addr)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.DeliveryAddresses.AddOrUpdate(addr);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> AddBulkAddress(List<DeliveryAddress> addrs)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.DeliveryAddresses.AddRange(addrs);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> AddAddressList(IEnumerable<DeliveryAddress> addrList )
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.DeliveryAddresses.AddRange(addrList);
                return await context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// soft delete addr
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public async Task<int> Delete(DeliveryAddress addr)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                addr.ExpireTime = DateTime.UtcNow;
                context.DeliveryAddresses.AddOrUpdate(addr);
                return await context.SaveChangesAsync();
            }
        }
    }
}
