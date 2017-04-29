using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.CustomerModel;
using DatanetCMS.Repository;

namespace DatanetCMS.Service
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ManagerRepository _managerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
            _managerRepository = new ManagerRepository();
        }

        /// <summary>
        /// Get all customer items
        /// </summary>
        /// <returns></returns>
        public async Task<MulitViewResult<CustomerModel>> GetAll()
        {
            var result = new MulitViewResult<CustomerModel>();
            try
            {
                var customers = await _customerRepository.GetAll();
                result.Datas = customers.Select(x => x.ToCustomerModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "GetAll", ex);
            }
            return result;
        }

        public async Task<MulitViewResult<CustomerModel>> GetAllName()
        {
            var result = new MulitViewResult<CustomerModel>();
            try
            {
                var customers = await _customerRepository.GetAll();
                foreach (var item in customers)
                {
                    result.Datas.Add(new CustomerModel()
                    {
                        CompanyName = item.CompanyName,
                        ContactName = item.ContactName
                    });
                }
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "GetAllName", ex);
            }
            return result;
        }

        /// <summary>
        /// Get customer item by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<CustomerModel>> GetById(int id)
        {
            var result = new ViewResult<CustomerModel>();
            try
            {
                var customer = await _customerRepository.GetById(id);
                result.Data = customer?.ToCustomerModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "GetById", ex);
            }
            return result;
        }

        /// <summary>
        /// Add or update a customer model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ViewResult<CustomerModel>> AddOrUpdate(CustomerModel model, string userName)
        {
            var result = new ViewResult<CustomerModel>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //check duplicated name
                    var dupCustomer = await _customerRepository.GetCustomerByCompName(model.CompanyName,model.Id);
                    if (dupCustomer!=null)
                    {
                        result.Status = -2;
                        result.Message = "Company name is already taken";
                        return result;
                    }
                    var dupCustomerLoginId = await _customerRepository.GetCustomerByLoginId(model.LoginId, model.Id);
                    if (dupCustomerLoginId != null)
                    {
                        result.Status = -2;
                        result.Message = "Login Id is already taken";
                        return result;
                    }
                    var dupName = await _managerRepository.GetManagerByName(model.LoginId, 0);
                    if (dupName != null)
                    {
                        result.Status = -2;
                        result.Message = "Login Id is already taken";
                        return result;
                    }

                    Customer customer;
                    //new customer model
                    if (model.Id == 0)
                    {
                        customer = model.ToCustomerModel();
                        customer.CreateTime = DateTime.UtcNow;
                        customer.CreateBy = userName;
                    }
                    else
                    {
                        var customerModel = await _customerRepository.GetById(model.Id);
                        if (customerModel == null)
                        {
                            result.Status = -3;
                            result.Message = "UOM does not exist";
                            return result;
                        }

                        customer = model.ToCustomerModel();
                        customer.CreateTime = customerModel.CreateTime;
                        customer.CreateBy = customerModel.CreateBy;
                        customer.EditTime = DateTime.UtcNow;
                        customer.EditBy = userName;
                    }
                    await _customerRepository.AddOrUpdate(customer);

                    var customerResult = await _customerRepository.GetById(customer.Id);

                    result.Data = customerResult.ToCustomerModel();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "AddOrUpdate", ex);
            }
            return result;
        }

        /// <summary>
        /// Delete cusomter model
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
                    var customer = await _customerRepository.GetById(id);
                    if (customer == null)
                    {
                        result.Status = -2;
                        result.Message = "Customer does not exist";
                        return result;
                    }
                    await _customerRepository.Delete(customer);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "DeleteById", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageModel<CustomerModel>>> Search(int? page, int? pageSize, string filter = null)
        {
            var result = new ViewResult<PageModel<CustomerModel>>();
            try
            {
                var customers = await _customerRepository.GetAll();
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<CustomerModel> customerModels = null;
                int totalCustomerModels;
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    customerModels = customers.Select(x => x.ToCustomerModelWithourLogo()).Where(c => c.CompanyName.ToLower().Contains(filter)).OrderBy(c => c.CompanyName)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalCustomerModels = customers.Count(c => c.CompanyName.ToLower().Contains(filter));
                }
                else
                {
                    customerModels =
                        customers.Select(x => x.ToCustomerModelWithourLogo())
                            .OrderBy(c => c.CompanyName)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalCustomerModels = customers.Count();
                }
                result.Data = new PageModel<CustomerModel>
                {
                    Page = currentPage,
                    TotalCount = totalCustomerModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalCustomerModels / currentPageSize),
                    Items = customerModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "Search", ex);
            }
            return result;
        }

        /// <summary>
        /// Get customer item by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ViewResult<CustomerModel>> GetByName(string name)
        {
            var result = new ViewResult<CustomerModel>();
            try
            {
                var customer = await _customerRepository.GetByName(name);
                result.Data = customer?.ToCustomerModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "GetByName", ex);
            }
            return result;
        }

        public async Task<MulitViewResult<CustomerModel>> GetByManagerId(int id)
        {
            var result = new MulitViewResult<CustomerModel>();
            try
            {
                var customers = await _customerRepository.GetCustomersByManagerId(id);
                result.Datas = customers?.Select(x => x.ToCustomerModelSimple()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CustomerService", "GetAllName", ex);
            }
            return result;
        }
    }
}
