using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.CustomerModel;
using DatanetCMS.Model.ManagerModel;
using DatanetCMS.Repository;
using System.Configuration;

namespace DatanetCMS.Service
{
    public class ManagerService
    {
        private readonly ManagerRepository _managerRepository;
        private readonly CustomerRepository _customerRepository;

        public ManagerService()
        {
            _managerRepository = new ManagerRepository();
            _customerRepository = new CustomerRepository();
        }
        /// <summary>
        /// search and filter manager
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ViewResult<PageModel<ManagerModel>>> Search(int? page, int? pageSize, string filter = null)
        {
            var result = new ViewResult<PageModel<ManagerModel>>();
            try
            {
                var managers = await _managerRepository.GetAll();
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<ManagerModel> managerModels = null;
                int totalManagerModels;
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    managerModels = managers.Select(x => x.ToManagerModel()).Where(c => c.Name.ToLower().Contains(filter)).OrderBy(c => c.Name)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalManagerModels = managers.Count(c => c.Name.ToLower().Contains(filter));
                }
                else
                {
                    managerModels =
                        managers.Select(x => x.ToManagerModel())
                            .OrderBy(c => c.Name)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalManagerModels = managers.Count();
                }
                result.Data = new PageModel<ManagerModel>
                {
                    Page = currentPage,
                    TotalCount = totalManagerModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalManagerModels / currentPageSize),
                    Items = managerModels
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
        /// Get all manager items
        /// </summary>
        /// <returns></returns>
        public async Task<MulitViewResult<ManagerModel>> GetAll()
        {
            var result = new MulitViewResult<ManagerModel>();
            try
            {
                var manangers = await _managerRepository.GetAll();
                result.Datas = manangers.Select(x => x.ToManagerModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ManagerService", "GetAll", ex);
            }
            return result;
        }
        /// <summary>
        /// get manager by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<ManagerModel>> GetById(int id)
        {
            var result = new ViewResult<ManagerModel>();
            try
            {
                var mananger = await _managerRepository.GetById(id);
                result.Data = mananger.ToManagerModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ManagerService", "GetById", ex);
            }
            return result;
        }
        /// <summary>
        /// add or update manager
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ViewResult<ManagerModel>> AddOrUpdate(ManagerModel model, string userName)
        {
            var result = new ViewResult<ManagerModel>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //check duplicated name
                    var dupCustomer = await _customerRepository.GetCustomerByLoginId(model.LoginName);
                    var dupName = await _managerRepository.GetManagerByName(model.LoginName, model.Id);
                    var dupManagerName = await _managerRepository.GetManagerByManagerName(model.Name, model.Id);
                    if (dupManagerName!=null)
                    {
                        result.Status = -2;
                        result.Message = "Name is already taken";
                        return result;
                    }
                    if (dupCustomer != null || dupName != null)
                    {
                        result.Status = -2;
                        result.Message = "Login Id is already taken";
                        return result;
                    }

                    Manager manager;
                    //new manager model
                    if (model.Id == 0)
                    {
                        manager = model.ToManagerModel();
                        manager.CreateTime = DateTime.UtcNow;
                        manager.CreateBy = userName;
                    }
                    else
                    {
                        var managerModel = await _managerRepository.GetById(model.Id);
                        if (managerModel == null)
                        {
                            result.Status = -3;
                            result.Message = "Manager does not exist";
                            return result;
                        }

                        var customer = await _customerRepository.GetByManagerId(model.Id);
                        if (customer != null && managerModel.Role == "Manager" && model.Role == "Admin")
                        {
                            result.Status = -3;
                            result.Message = $"Manager is used by {customer.CompanyName}";
                            return result;
                        }

                        manager = model.ToManagerModel();
                        manager.CreateTime = managerModel.CreateTime;
                        manager.CreateBy = managerModel.CreateBy;
                        manager.EditTime = DateTime.UtcNow;
                        manager.EditBy = userName;
                    }
                    await _managerRepository.AddOrUpdate(manager);

                    var managerResult = await _managerRepository.GetById(manager.Id);

                    result.Data = managerResult.ToManagerModel();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ManagerService", "AddOrUpdate", ex);
            }
            return result;
        }

        public async Task<Operate> DeleteById(int id)
        {
            var result = new Operate();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var manager = await _managerRepository.GetById(id);
                    if (manager == null)
                    {
                        result.Status = -2;
                        result.Message = "Manager does not exist";
                        return result;
                    }

                    //check wether customer is using mananger
                    var customer = await _customerRepository.GetByManagerId(id);
                    if (customer != null)
                    {
                        result.Status = -3;
                        result.Message = $"Manager is used by {customer.CompanyName}";
                        return result;
                    }

                    await _managerRepository.Delete(manager);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ManagerService", "DeleteById", ex);
            }
            return result;
        }

        public async Task CheckAdmin()
        {
            var customer = await _managerRepository.GetAdmin();
            if(customer == null)
            {
                var admin = new Manager
                {
                    Id = 0,
                    Name = ConfigurationManager.AppSettings["AdminName"] ?? "admin",
                    LoginName = ConfigurationManager.AppSettings["AdminLoginName"] ?? "admin",
                    Password = ConfigurationManager.AppSettings["AdminPwd"] ?? "admin",
                    Role = "Admin",
                    Phone = ConfigurationManager.AppSettings["AdminPhone"] ?? "",
                    Email = ConfigurationManager.AppSettings["AdminEmail"] ?? "",
                    CreateBy = "machine",
                    CreateTime = DateTime.UtcNow,
                };
                await _managerRepository.AddOrUpdate(admin);
            }
        }
    }
}
