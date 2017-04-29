using DatanetCMS.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace DatanetCMS.Repository
{
    public class CustomerRepository
    {
        public CustomerRepository()
        {
            
        }

        /// <summary>
        /// get all customer info which isn't soft deleted
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.ExpireTime == null)
                    .Include(x=>x.BuyerGroup)
                    .Include(x=>x.Manager)
                    .OrderBy(x=>x.CompanyName)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// get customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.Id == id && x.ExpireTime == null)
                    .Include(x=>x.BuyerGroup)
                    .Include(x => x.Manager)
                    .FirstOrDefaultAsync();
            }
        }
        public async Task<List<Customer>> GetByBuyerGroupId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.BuyerGroupId == id && x.ExpireTime == null).ToListAsync();
            }
        }

        /// <summary>
        /// soft delete customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<int> Delete(Customer customer)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                customer.ExpireTime = DateTime.UtcNow;
                context.Customers.AddOrUpdate(customer);
                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// add or update customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(Customer customer)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Customers.AddOrUpdate(customer);
                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// get customer by company name
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="loginId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByCompName(string comName,int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x=>x.CompanyName == comName && x.Id != id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// get customer by login id
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="loginId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByLoginId(string loginId, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.LoginId == loginId && x.Id != id&&x.ExpireTime==null).FirstOrDefaultAsync();
            }
        }
        public async Task<Customer> GetCustomerByLoginId(string loginId)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.LoginId == loginId && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }
        public async Task<Customer> GetByName(string name)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers
                    .Include(x=>x.Manager)
                    .Where(x => x.LoginId == name && x.ExpireTime == null)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<Customer> GetByManagerId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.ManagerId == id && x.ExpireTime == null)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<List<Customer>> GetCustomersByManagerId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Customers.Where(x => x.ManagerId == id && x.ExpireTime == null)
                    .ToListAsync();
            }
        }
    }
}
