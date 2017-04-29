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
    public class ContractNumberRepository
    {
        public ContractNumberRepository() { }

        /// <summary>
        /// get all contract info which isn't soft deleted
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContractNumber>> GetAll()
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.ContractNumbers.Where(x => x.ExpireTime == null)
                    .Include(x => x.Customer).OrderBy(x=>x.Name)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// get contract numbers by customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ContractNumber>> GetByCustomerId(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.ContractNumbers.Where(x => x.CustomerId == id && x.ExpireTime == null)
                    .Include(x => x.Customer).ToListAsync();
            }
        }

        public async Task<ContractNumber> GetById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.ContractNumbers.Where(x => x.Id == id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }

        public async Task<int> AddOrUpdate(ContractNumber contract)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.ContractNumbers.AddOrUpdate(contract);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> Delete(ContractNumber contract)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                contract.ExpireTime = DateTime.UtcNow;
                context.ContractNumbers.AddOrUpdate(contract);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<List<ContractNumber>> GetContractsByCustomerId(int customerId)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.ContractNumbers.Where(x => x.ExpireTime == null && x.CustomerId == customerId).ToListAsync();
            }
        }

        public async Task<ContractNumber> GetContractByName(string name, int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.ContractNumbers.Where(x => x.Name == name && x.Id != id && x.ExpireTime == null).FirstOrDefaultAsync();
            }
        }
    }
}
