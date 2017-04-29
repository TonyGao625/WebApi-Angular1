using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.Common.Log;
using DatanetCMS.Model;
using DatanetCMS.Model.UomModel;
using DatanetCMS.Repository;
using System.Transactions;
using DatanetCMS.DAO;

namespace DatanetCMS.Service
{
    public class UomService
    {
        private readonly UomRepository _uomRepository;
        private readonly ProductRepository _productRepository;
        public UomService()
        {
            _uomRepository = new UomRepository();
            _productRepository = new ProductRepository();
        }

        public async Task<ViewResult<PageUomModel>> Search(int? page, int? pageSize, string filter = null)
        {
            var result = new ViewResult<PageUomModel>();
            try
            {
                var uoms = await _uomRepository.GetAll();
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<UomModel> uomModels=null;
                int totalUomModels =new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    uomModels= uoms.Select(x => x.ToUomModel()).Where(c=>c.Name.ToLower().Contains(filter)).OrderBy(c => c.Name)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalUomModels = uoms.Count(c => c.Name.ToLower().Contains(filter));
                }
                else
                {
                    uomModels =
                        uoms.Select(x => x.ToUomModel())
                            .OrderBy(c => c.Name)
                            .Skip(currentPage*currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalUomModels = uoms.Count();
                }
                result.Data =new PageUomModel()
                {
                    Page = currentPage,
                    TotalCount = totalUomModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalUomModels / currentPageSize),
                    Items = uomModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("UomService", "Search", ex);
            }
            return result;
        }

        /// <summary>
        /// Get all uom items
        /// </summary>
        /// <returns></returns>
        public async Task<MulitViewResult<UomModel>> GetAll()
        {
            var result = new MulitViewResult<UomModel>();
            try
            {
                var uoms = await _uomRepository.GetAll();
                result.Datas = uoms.Select(x => x.ToUomModel()).ToList();
                result.AllCount = result.Datas?.Count??0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("UomService", "GetAll", ex);
            }
            return result;
        }

        /// <summary>
        /// Get uom item by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<UomModel>> GetById(int id)
        {
            var result = new ViewResult<UomModel>();
            try
            {
                var uom = await _uomRepository.GetById(id);
                result.Data = uom?.ToUomModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("UomService", "GetById", ex);
            }
            return result;
        }

        /// <summary>
        /// Add or update a uom model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ViewResult<UomModel>> AddOrUpdate(UomModel model, string userName)
        {
            var result = new ViewResult<UomModel>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //check duplicated name
                    var dupUom = await _uomRepository.GetUomByName(model.Name, model.Id);
                    if (dupUom != null)
                    {
                        result.Status = -2;
                        result.Message = "Name is already taken";
                        return result;
                    }

                    Uom uom;
                    //new uom model
                    if (model.Id == 0)
                    {
                        uom = model.ToUomModel();
                        uom.CreateTime = DateTime.UtcNow;
                        uom.CreateBy = userName;
                    }
                    else
                    {
                        uom = await _uomRepository.GetById(model.Id);
                        if (uom == null)
                        {
                            result.Status = -3;
                            result.Message = "UOM does not exist";
                            return result;
                        }
                        uom.EditTime = DateTime.UtcNow;
                        uom.EditBy = userName;
                        uom.Name = model.Name;
                    }
                    await _uomRepository.AddOrUpdate(uom);

                    result.Data = uom.ToUomModel();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("UomService", "AddOrUpdate", ex);
            }
            return result;
       }

        /// <summary>
        /// Delete uom model
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
                    var uom = await _uomRepository.GetById(id);
                    if (uom == null)
                    {
                        result.Status = -2;
                        result.Message = "Uom does not exist";
                        return result;
                    }

                    var product = await _productRepository.GetProductByUomId(id);
                    if (product != null)
                    {
                        result.Status = -3;
                        result.Message = "Uom is used by product";
                        return result;
                    }

                    await _uomRepository.Delete(uom);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("UomService", "DeleteById", ex);
            }
            return result;
        }
    }
}
