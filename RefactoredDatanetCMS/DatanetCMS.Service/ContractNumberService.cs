using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.ContractNumberModel;
using DatanetCMS.Model.DeliveryAddrModel;
using DatanetCMS.Repository;

namespace DatanetCMS.Service
{
    public class ContractNumberService
    {
        private readonly ContractNumberRepository _contractNumberRepository;

        public ContractNumberService()
        {
            _contractNumberRepository = new ContractNumberRepository();
        }

        /// <summary>
        /// Get all contract number items
        /// </summary>
        /// <returns></returns>
        public async Task<MulitViewResult<ContractNumberModel>> GetAll()
        {
            var result = new MulitViewResult<ContractNumberModel>();
            try
            {
                var cns = await _contractNumberRepository.GetAll();
                result.Datas = cns.Select(x => x.ToContractNumberModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ContractNumberService", "GetAll", ex);
            }
            return result;
        }
        /// <summary>
        /// get contract number list by customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MulitViewResult<ContractNumberModel>> GetByCustomerId(int id)
        {
            var result = new MulitViewResult<ContractNumberModel>();
            try
            {
                var cns = await _contractNumberRepository.GetByCustomerId(id);
                result.Datas = cns.Select(x => x.ToContractNumberModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ContractNumberService", "GetByCustomerId", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageModel<ContractNumberModel>>> Search(int? page, int? pageSize, int customerId, string filter = null)
        {
            var result = new ViewResult<PageModel<ContractNumberModel>>();
            try
            {
                var contracts = await _contractNumberRepository.GetByCustomerId(customerId);
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<ContractNumberModel> contractModels = null;
                int totalContractModels = new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    contractModels = contracts.Select(x => x.ToContractNumberModel()).Where(c => c.Name.ToLower().Contains(filter)).OrderBy(c => c.Name)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalContractModels = contracts.Count(c => c.Name.ToLower().Contains(filter));
                }
                else
                {
                    contractModels =
                        contracts.Select(x => x.ToContractNumberModel())
                            .OrderBy(c => c.Name)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalContractModels = contracts.Count();
                }
                result.Data = new PageModel<ContractNumberModel>()
                {
                    Page = currentPage,
                    TotalCount = totalContractModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalContractModels / currentPageSize),
                    Items = contractModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ContractNumberService", "Search", ex);
            }
            return result;
        }
        public async Task<ViewResult<ContractNumberModel>> AddOrUpdate(ContractNumberModel model, string userName)
        {
            var result = new ViewResult<ContractNumberModel>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ContractNumber contractNumber;
                    var contractNumberTempByName =
     await _contractNumberRepository.GetContractByName(model.Name, model.Id);
                    if (contractNumberTempByName != null)
                    {
                        result.Status = -2;
                        result.Message = "Contract number is already taken";
                        return result;
                    }
                    if (model.Id == 0)
                    {
                        contractNumber = model.ToContractNumberModel();
                        contractNumber.CreateTime = DateTime.UtcNow;
                        contractNumber.CreateBy = userName;
                    }
                    else
                    {
                        var contractNumberTemp = await _contractNumberRepository.GetById(model.Id);
                        if (contractNumberTemp == null)
                        {
                            result.Status = -3;
                            result.Message = "This contract does not exist";
                            return result;
                        }
                        contractNumber = model.ToContractNumberModel();
                        contractNumber.CreateTime = contractNumberTemp.CreateTime;
                        contractNumber.CreateBy = contractNumberTemp.CreateBy;
                        contractNumber.EditTime = DateTime.UtcNow;
                        contractNumber.EditBy = userName;
                    }
                    await _contractNumberRepository.AddOrUpdate(contractNumber);

                    result.Data = contractNumber.ToContractNumberModel();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ContractNumberService", "AddOrUpdate", ex);
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
                    var contract = await _contractNumberRepository.GetById(id);
                    if (contract == null)
                    {
                        result.Status = -2;
                        result.Message = "This contract does not exist";
                        return result;
                    }
                    await _contractNumberRepository.Delete(contract);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ContractNumberService", "DeleteById", ex);
            }
            return result;
        }

        public async Task<MulitViewResult<ContractNumberModel>> GetContractsByCustomerId(int customerId)
        {
            var result = new MulitViewResult<ContractNumberModel>();
            try
            {
                var contracts = await _contractNumberRepository.GetContractsByCustomerId(customerId);
                result.Datas = contracts.Select(x => x.ToContractNumberModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ContractNumberService", "GetContractsByCustomerId", ex);
            }
            return result;
        }
    }
}
