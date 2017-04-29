using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using System;

namespace DatanetCMS.Repository
{
    public class OrderRepositroy
    {
        /// <summary>
        /// add or update order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(Order order)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                context.Orders.AddOrUpdate(order);
                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// get customer's orders or quotes by mode and order code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrdersByCustomerId(int id, string mode, string code, string dateFilter)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                var result = context.Orders.Where(x=>x.CustomerId == id && x.Mode == mode && x.IsBuildOrder != true);
               
                if (!string.IsNullOrWhiteSpace(code))
                {
                    if (mode == "9")
                    {
                        result = result.Where(x => x.QuoteCode != null && x.QuoteCode.Contains(code));
                    }
                    else if (mode == "7")
                    {
                        result = result.Where(x => x.OrderCode != null && x.OrderCode.Contains(code));
                    }
                    else
                    {
                        throw new Exception("Mode is not valid");
                    }
                }
                if (!string.IsNullOrWhiteSpace(dateFilter))
                {
                    var fromDate = DateTime.UtcNow;
                    if(dateFilter == "1")
                    {
                        fromDate = fromDate.AddYears(-1);
                    }
                    else if(dateFilter == "2")
                    {
                        fromDate = fromDate.AddMonths(-1);
                    }
                    else if(dateFilter == "3")
                    {
                        fromDate = fromDate.AddDays(-7);
                    }
                    else
                    {
                        fromDate = DateTime.MinValue;
                    }
                    result = result.Where(x=>x.CreateTime > fromDate);
                }
                return await result.ToListAsync();
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Orders.FirstOrDefaultAsync(x => x.Id == id && x.Mode != "0");
            }
        }

        public async Task<Order> GetOrderOrQuoteById(int id)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                return await context.Orders
                    .Include(x => x.OrderProducts)
                    .Include(x => x.OrderProducts.Select(y => y.Product))
                    .Include(x => x.OrderProducts.Select(y => y.Product.Uom))
                    .Include(x => x.OrderProducts.Select(y => y.Product.Category))
                    .Include(x => x.OrderAddresses)
                    .Where(x => x.Id == id && x.IsBuildOrder != true)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<List<Order>> GetAllByFilter(FilterModel filterModel)
        {
            using (DatanetCMSWebEntities context = new DatanetCMSWebEntities())
            {
                IQueryable<Order> result = context.Orders;
                if (!string.IsNullOrWhiteSpace(filterModel.Mode))
                {
                    if(filterModel.Mode == "1")
                    {
                        result = result.Where(x=>x.Mode == "7" || x.Mode == "9");
                        if (!string.IsNullOrWhiteSpace(filterModel.OrderCode))
                        {
                            result = result.Where(x=>(x.QuoteCode!=null&&x.QuoteCode.Contains(filterModel.OrderCode))||
                            (x.OrderCode!=null && x.OrderCode.Contains(filterModel.OrderCode)));
                        }
                    }
                    else
                    {
                        result = result.Where(x => x.Mode == filterModel.Mode);
                        
                        if (!string.IsNullOrWhiteSpace(filterModel.OrderCode))
                        {
                            if (filterModel.Mode == "7")//order
                            {
                                result = result.Where(x => x.OrderCode!=null&&x.OrderCode.Contains(filterModel.OrderCode));
                            }
                            if(filterModel.Mode == "9")
                            {
                                result = result.Where(x => x.QuoteCode != null && x.QuoteCode.Contains(filterModel.OrderCode));
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(filterModel.CompanyName))
                {
                    result = result.Where(x=>x.CompanyName!=null && x.CompanyName.Contains(filterModel.CompanyName));
                }
                if (!string.IsNullOrWhiteSpace(filterModel.ContactName))
                {
                    result = result.Where(x=>x.ContactName!=null && x.ContactName.Contains(filterModel.ContactName));
                }
                DateTime fromDate, toDate;
                var fromDateFlag = DateTime.TryParse(filterModel.FromDate, out fromDate);
                var toDateFlag = DateTime.TryParse(filterModel.ToDate, out toDate);
                if (fromDateFlag)
                {
                    result = result.Where(x=>x.CreateTime >= fromDate);
                }
                if (toDateFlag)
                {
                    toDate = toDate.AddDays(1);
                    result = result.Where(x=>x.CreateTime <= toDate);
                }
                if(filterModel.ManagerId > 0)
                {
                    result = result.Where(x=>x.ManagerId == filterModel.ManagerId);
                }
                result = result.Where(x=>x.IsBuildOrder != true);
                return await result.ToListAsync();
            }
        }
    }
}
