using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.UomModel;
using DatanetCMS.Repository;
using System.Linq;
using DatanetCMS.Model.DeliveryAddrModel;

namespace DatanetCMS.Service
{
    public class DeliveryAddrService
    {
        private readonly DeliveryAddressRepository _deliveryAddrRepository;
        private readonly CustomerRepository _customerRepository;
        public DeliveryAddrService()
        {
            _deliveryAddrRepository = new DeliveryAddressRepository();
            _customerRepository = new CustomerRepository();
        }

        /// <summary>
        /// search by postcode
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="customerId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ViewResult<PageModel<DeliveryAddrModel>>> Search(int? page, int? pageSize, int customerId, string filter = null)
        {
            var result = new ViewResult<PageModel<DeliveryAddrModel>>();
            try
            {
                var addrs = await _deliveryAddrRepository.GetAddrsByCustomerId(customerId);
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<DeliveryAddrModel> deliveryAddrModel = null;
                int totalAddrModels = new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    deliveryAddrModel = addrs.Select(x => x.ToDeliveryAddrModel()).Where(c => c.PostCode.ToLower().Contains(filter)).OrderBy(c => c.PostCode)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalAddrModels = addrs.Count(c => c.PostCode.ToLower().Contains(filter));
                }
                else
                {
                    deliveryAddrModel =
                        addrs.Select(x => x.ToDeliveryAddrModel())
                            .OrderBy(c => c.PostCode)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalAddrModels = addrs.Count();
                }
                result.Data = new PageModel<DeliveryAddrModel>()
                {
                    Page = currentPage,
                    TotalCount = totalAddrModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalAddrModels / currentPageSize),
                    Items = deliveryAddrModel
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "Search", ex);
            }
            return result;
        }

        /// <summary>
        /// Get all addr items
        /// </summary>
        /// <returns></returns>
        public async Task<MulitViewResult<DeliveryAddrModel>> GetAll()
        {
            var result = new MulitViewResult<DeliveryAddrModel>();
            try
            {
                var addrs = await _deliveryAddrRepository.GetAll();
                result.Datas = addrs.Select(x => x.ToDeliveryAddrModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "GetAll", ex);
            }
            return result;
        }

        /// <summary>
        /// get addrs by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<MulitViewResult<DeliveryAddrModel>> GetAddrsByCustomerId(int customerId)
        {
            var result = new MulitViewResult<DeliveryAddrModel>();
            try
            {
                var addrs = await _deliveryAddrRepository.GetAddrsByCustomerId(customerId);
                result.Datas = addrs.Select(x => x.ToDeliveryAddrModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "GetAll", ex);
            }
            return result;
        }

        /// <summary>
        /// Get addr item by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<DeliveryAddrModel>> GetById(int id)
        {
            var result = new ViewResult<DeliveryAddrModel>();
            try
            {
                var addr = await _deliveryAddrRepository.GetById(id);
                result.Data = addr?.ToDeliveryAddrModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "GetById", ex);
            }
            return result;
        }

        /// <summary>
        /// Add or update a addr model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ViewResult<DeliveryAddrModel>> AddOrUpdate(DeliveryAddrModel model, string userName)
        {
            var result = new ViewResult<DeliveryAddrModel>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    DeliveryAddress deliveryAddr;
                    //new uom model
                    if (model.Id == 0)
                    {
                        deliveryAddr = model.ToDeliveryAddrModel();
                        deliveryAddr.CreateTime = DateTime.UtcNow;
                        deliveryAddr.CreateBy = userName;
                    }
                    else
                    {
                        var deliveryAddrTemp = await _deliveryAddrRepository.GetById(model.Id);
                        if (deliveryAddrTemp == null)
                        {
                            result.Status = -3;
                            result.Message = "This address does not exist";
                            return result;
                        }
                        deliveryAddr = model.ToDeliveryAddrModel();
                        deliveryAddr.CreateTime = deliveryAddrTemp.CreateTime;
                        deliveryAddr.CreateBy = deliveryAddrTemp.CreateBy;
                        deliveryAddr.EditTime = DateTime.UtcNow;
                        deliveryAddr.EditBy = userName;
                    }
                    await _deliveryAddrRepository.AddOrUpdate(deliveryAddr);

                    result.Data = deliveryAddr.ToDeliveryAddrModel();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "AddOrUpdate", ex);
            }
            return result;
        }

        public async Task<Operate> addBulkAddress(DeliveryAddrBulkModel model, string userName)
        {
            var result = new Operate();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var deliveryAddrs=new List<DeliveryAddress>();
                    foreach (var item in model.DeliveryAddrModel)
                    {
                        DeliveryAddress deliveryAddr;
                        deliveryAddr = item.ToDeliveryAddrModel();
                        deliveryAddr.CreateTime = DateTime.UtcNow;
                        deliveryAddr.CreateBy = userName;
                        deliveryAddrs.Add(deliveryAddr);
                    }
                    await _deliveryAddrRepository.AddBulkAddress(deliveryAddrs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "AddOrUpdate", ex);
            }
            return result;
        }
        public async Task<ViewResult<Operate>> AddAddressList(DeliveryAddrModel model, string userName, int customerId)
        {
            var result = new ViewResult<Operate>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    DeliveryAddress deliveryAddr;
                    //new uom model
                    if (model.Id == 0)
                    {
                        deliveryAddr = model.ToDeliveryAddrModel();
                        deliveryAddr.CreateTime = DateTime.UtcNow;
                        deliveryAddr.CreateBy = userName;
                    }
                    else
                    {
                        var deliveryAddrTemp = await _deliveryAddrRepository.GetById(model.Id);
                        if (deliveryAddrTemp == null)
                        {
                            result.Status = -3;
                            result.Message = "This address does not exist";
                            return result;
                        }
                        deliveryAddr = model.ToDeliveryAddrModel();
                        deliveryAddr.CreateTime = deliveryAddrTemp.CreateTime;
                        deliveryAddr.CreateBy = deliveryAddrTemp.CreateBy;
                        deliveryAddr.EditTime = DateTime.UtcNow;
                        deliveryAddr.EditBy = userName;
                    }
                    await _deliveryAddrRepository.AddOrUpdate(deliveryAddr);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "AddOrUpdate", ex);
            }
            return result;
        }

        /// <summary>
        /// Delete addr model
        /// NOTE: soft delete 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Operate> DeleteById(int id)
        {
            var result = new Operate();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var addr = await _deliveryAddrRepository.GetById(id);
                    if (addr == null)
                    {
                        result.Status = -2;
                        result.Message = "This address does not exist";
                        return result;
                    }

                    await _deliveryAddrRepository.Delete(addr);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("DeliveryAddrService", "DeleteById", ex);
            }
            return result;
        }
    }
}
