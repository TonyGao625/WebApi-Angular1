using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.BuyerGroupModel;
using DatanetCMS.Repository;

namespace DatanetCMS.Service
{
    public class BuyerGroupService
    {
        private readonly BuyerGroupRepository _buyerGroupRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        public BuyerGroupService()
        {
            _buyerGroupRepository = new BuyerGroupRepository();
            _customerRepository = new CustomerRepository();
            _productRepository=new ProductRepository();
        }
        public async Task<MulitViewResult<BuyerGroupModel>> GetAll()
        {
            var result = new MulitViewResult<BuyerGroupModel>();
            try
            {
                var buyerGroups = await _buyerGroupRepository.GetAll();
                result.Datas = buyerGroups.Select(x => x.ToBuyerGroupModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("BuyerGroupService", "GetAll", ex);
            }
            return result;
        }
        public async Task<ViewResult<GroupProductModel>> GetById(int id)
        {
            var result = new ViewResult<GroupProductModel>();
            try
            {
                var buyerGroup = await _buyerGroupRepository.GetById(id);
                var Id = buyerGroup.Id;
                var code = buyerGroup.Code;
                var customerTempModelList=new List<CustomerTempModel>();
                var productTempModelList=new List<ProductTempModel>();
                var customerList= await _customerRepository.GetByBuyerGroupId(buyerGroup.Id);
                foreach (var customerItem in customerList)
                {
                    customerTempModelList.Add(new CustomerTempModel()
                    {
                        Id =customerItem.Id,
                        Name = customerItem.CompanyName
                    });
                }
                var groupProductList = await _buyerGroupRepository.GetGroupProductByBuyerGroupId(id);
                foreach (var groupProductItem in groupProductList)
                {
                    if (groupProductItem.ProductId != null)
                    {
                        var product = await _productRepository.GetById((int)groupProductItem.ProductId);
                        if (product!=null)
                        {
                            productTempModelList.Add(new ProductTempModel()
                            {
                                ProductId = (int)groupProductItem.ProductId,
                                Price = groupProductItem.Price,
                                Code = product.Code
                            });
                        }
                    }
                }
                result.Data=new GroupProductModel()
                {
                    Id=Id,
                    Code = code,
                    CustomerTempModelList = customerTempModelList,
                    ProductTempModelList = productTempModelList
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("BuyerGroupService", "GetAll", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageBuyerGroupModel>> Search(int? page, int? pageSize, string filter = null)
        {
            var result = new ViewResult<PageBuyerGroupModel>();
            try
            {
                var buyerGroups = await _buyerGroupRepository.GetAll();
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<BuyerGroupModel> buyerGroupModels = null;
                int totalBuyerGroupModels = new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    buyerGroupModels = buyerGroups.Select(x => x.ToBuyerGroupModel()).Where(c => c.Code.ToLower().Contains(filter)).OrderBy(c => c.Code)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalBuyerGroupModels = buyerGroups.Count(c => c.Code.ToLower().Contains(filter));
                }
                else
                {
                    buyerGroupModels =
                        buyerGroups.Select(x => x.ToBuyerGroupModel())
                            .OrderBy(c => c.Code)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalBuyerGroupModels = buyerGroups.Count();
                }
                result.Data = new PageBuyerGroupModel()
                {
                    Page = currentPage,
                    TotalCount = totalBuyerGroupModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalBuyerGroupModels / currentPageSize),
                    Items = buyerGroupModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("BuyerGroupService", "Search", ex);
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
                    var buyerGroup = await _buyerGroupRepository.GetById(id);
                    if (buyerGroup == null)
                    {
                        result.Status = -2;
                        result.Message = "Buyer Group does not exist";
                        return result;
                    }
                    await _buyerGroupRepository.DeleteBuyerGroup(buyerGroup);
                    var customerList = await _customerRepository.GetByBuyerGroupId(buyerGroup.Id);
                    foreach (var customer in customerList)
                    {
                        customer.BuyerGroupId = 0;
                        await _customerRepository.AddOrUpdate(customer);
                    }
                    var groupProductList = await _buyerGroupRepository.GetGroupProductByBuyerGroupId(buyerGroup.Id);
                    foreach (var groupProduct in groupProductList)
                    {
                        groupProduct.ExpireTime = DateTime.UtcNow;
                        await _buyerGroupRepository.AddOrUpdateGroupProduct(groupProduct);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("BuyerGroupService", "DeleteById", ex);
            }
            return result;
        }
        public async Task<Operate> AddOrUpdate(GroupProductModel model, string userName)
        {
            var result = new Operate();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //check duplicated group code
                    var dupGroup = await _buyerGroupRepository.GetGroupByName(model.Code, model.Id);
                    if (dupGroup != null)
                    {
                        result.Status = -2;
                        result.Message = "Name is already taken";
                        return result;
                    }
                    BuyerGroup buyerGroup;
                    if (model.Id == 0)
                    {
                        buyerGroup = new BuyerGroup()
                        {
                            Code = model.Code,
                            CreateBy = userName,
                            CreateTime = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        buyerGroup = await _buyerGroupRepository.GetById(model.Id);
                        if (buyerGroup == null)
                        {
                            result.Status = -3;
                            result.Message = "Buyer Group does not exist";
                            Logger.WriteErrorLog("BuyerGroupService", "AddOrUpdate", new Exception("Buyer Group does not exist"));
                            return result;
                        }
                        buyerGroup.Code = model.Code;
                        buyerGroup.EditTime = DateTime.UtcNow;
                        buyerGroup.EditBy = userName;
                    }
                    await _buyerGroupRepository.AddOrUpdate(buyerGroup);
                    //update customer table's buyergroupId
                    var customerIdList = new List<int>();
                    foreach (var customerItem in model.CustomerTempModelList)
                    {
                        customerIdList.Add(customerItem.Id);
                        var customer = await _customerRepository.GetById(customerItem.Id);
                        if (customer == null) continue;
                        customer.BuyerGroupId = buyerGroup.Id;
                        await _customerRepository.AddOrUpdate(customer);
                    }
                    var customerListByGroupPRoduct = await _customerRepository.GetByBuyerGroupId(buyerGroup.Id);
                    foreach (var customer in customerListByGroupPRoduct)
                    {
                        if (customerIdList.IndexOf(customer.Id) < 0)
                        {
                            customer.BuyerGroupId = 0;
                            await _customerRepository.AddOrUpdate(customer);
                        }
                    }
                    //if group has been altered, set expire time now and insert new mapping relation
                    var groupProduct = await _buyerGroupRepository.GetGroupProductByBuyerGroupId(buyerGroup.Id);
                    if (groupProduct.Count != 0)
                    {
                        foreach (var item in groupProduct)
                        {
                            await _buyerGroupRepository.DeleteGroupProduct(item);
                        }
                    }
                    foreach (var productItem in model.ProductTempModelList)
                    {
                        var groupProductNew = new GroupProduct()
                        {
                            Id = 0,
                            BuyerGroupId = buyerGroup.Id,
                            ProductId = productItem.ProductId,
                            Price = productItem.Price
                        };
                        await _buyerGroupRepository.AddOrUpdateGroupProduct(groupProductNew);
                    }
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("BuyerGroupService", "AddOrUpdate", ex);
            }
            return result;
        }
    }
}
