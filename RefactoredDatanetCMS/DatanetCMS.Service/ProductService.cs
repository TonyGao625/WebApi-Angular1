using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.CategoryModel;
using DatanetCMS.Model.ProductModel;
using DatanetCMS.Model.UomModel;
using DatanetCMS.Repository;

namespace DatanetCMS.Service
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly BuyerGroupRepository _buyerGroupRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
            _customerRepository = new CustomerRepository();
            _buyerGroupRepository = new BuyerGroupRepository();
        }

        public async Task<ViewResult<PageProductModel>> Search(int? page, int? pageSize, string filter = null)
        {
            var result = new ViewResult<PageProductModel>();
            try
            {
                var productAlls = await _productRepository.GetAll();
                var products = from product in productAlls
                               select new Product()
                               {
                                   Id = product.Id,
                                   Code = product.Code,
                                   ShortDesc = product.ShortDesc,
                                   LongDesc = product.LongDesc,
                                   Vendor = product.Vendor,
                                   UomId = product.UomId,
                                   Uom = product.Uom,
                                   Category = product.Category,
                                   CategoryId = product.CategoryId
                               };
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<ProductModel> productModels = null;
                int totalProductModels = new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    productModels = products.Select(x => x.ToProductModel()).Where(c => c.Code.ToLower().Contains(filter)).OrderBy(c => c.Code)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalProductModels = products.Count(c => c.Code.ToLower().Contains(filter));
                }
                else
                {
                    productModels =
                        products.Select(x => x.ToProductModel())
                            .OrderBy(c => c.Code)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalProductModels = products.Count();
                }
                result.Data = new PageProductModel()
                {
                    Page = currentPage,
                    TotalCount = totalProductModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalProductModels / currentPageSize),
                    Items = productModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "Search", ex);
            }
            return result;
        }

        /// <summary>
        /// Get all product items
        /// </summary>
        /// <returns></returns>
        public async Task<MulitViewResult<ProductModel>> GetAll()
        {
            var result = new MulitViewResult<ProductModel>();
            try
            {
                var products = await _productRepository.GetAll();
                result.Datas = products.Select(x => x.ToProductModel()).ToList();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "GetAll", ex);
            }
            return result;
        }

        public async Task<MulitViewResult<ProductModel>> GetAllByCustomerId(int customerId, int categoryId)
        {
            var result = new MulitViewResult<ProductModel>();
            try
            {
                var customer = await _customerRepository.GetById(customerId);
                if (customer == null)
                {
                    result.Status = -2;
                    result.Message = "The customer does not exsit";
                    return result;
                }
                var groupProductList = await _buyerGroupRepository.GetGroupProductByBuyerGroupId(customer.BuyerGroupId ?? 0);
                if (groupProductList.Count == 0)
                {
                    result.Status = -3;
                    result.Message = "No products are available. Please contact your administrator ";
                    return result;
                }
                var productList = new List<ProductModel>();
                foreach (var groupProductItem in groupProductList)
                {
                    Product product;
                    if (categoryId == 0)
                    {
                        product = await _productRepository.GetById(groupProductItem.ProductId);
                    }
                    else
                    {
                        product = await _productRepository.GetByIdAndCategoryId(groupProductItem.ProductId, categoryId);
                    }
                    if (product != null)
                    {
                        productList.Add(new ProductModel()
                        {
                            Price = groupProductItem.Price,
                            Id = product.Id,
                            Code = product.Code,
                            ShortDesc = product.ShortDesc,
                            LongDesc = product.LongDesc,
                            Vendor = product.Vendor,
                            Uom = product.Uom.ToUomModel()
                        });
                    }
                }
                result.Datas = productList;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "GetAllByCustomerId", ex);
            }
            return result;
        }

        public async Task<MulitViewResult<CategoryModel>> GetAllCategoryByCustomerId(int customerId)
        {
            var result = new MulitViewResult<CategoryModel>();
            try
            {
                var customer = await _customerRepository.GetById(customerId);
                if (customer == null)
                {
                    result.Status = -2;
                    result.Message = "The customer does not exsit";
                    return result;
                }
                var groupProductList = await _buyerGroupRepository.GetGroupProductByBuyerGroupId(customer.BuyerGroupId ?? 0);
                if (groupProductList.Count == 0)
                {
                    result.Status = -3;
                    result.Message = "No products are available,Please contact your administrator.";
                    return result;
                }
                var categoryList = new List<CategoryModel>();
                var categoryIdList = new List<int>();
                foreach (var groupProductItem in groupProductList)
                {
                    var product = await _productRepository.GetById(groupProductItem.ProductId);
                    if (product?.Category == null) continue;
                    var tempCategoryModel = new CategoryModel()
                    {
                        Id = product.Category.Id,
                        CategoryName = product.Category?.CategoryName
                    };
                    if (categoryList.IndexOf(tempCategoryModel) >= 0) continue;
                    categoryIdList.Add(tempCategoryModel.Id);
                    categoryList.Add(tempCategoryModel);
                }
                result.Datas = categoryList;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "GetAllByCustomerId", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageProductModel>> SearchByCustomerId(int? page, int? pageSize, int customerId, int categoryId, string filter = null)
        {
            var result = new ViewResult<PageProductModel>();
            try
            {
                var products = await GetAllByCustomerId(customerId, categoryId);
                if (products.Status != 0)
                {
                    result.Status = products.Status;
                    result.Message = products.Message;
                    return result;
                }
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<ProductModel> productModels = null;
                int totalProductModels = new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    productModels = products.Datas.Where(c => c.Code.ToLower().Contains(filter)).OrderBy(c => c.Code)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalProductModels = products.Datas.Count(c => c.Code.ToLower().Contains(filter));
                }
                else
                {
                    productModels =
                        products.Datas
                            .OrderBy(c => c.Code)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalProductModels = products.Datas.Count();
                }
                result.Data = new PageProductModel()
                {
                    Page = currentPage,
                    TotalCount = totalProductModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalProductModels / currentPageSize),
                    Items = productModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "SearchByCustomerId", ex);
            }
            return result;
        }

        /// <summary>
        /// Get product item by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<ProductModel>> GetById(int id)
        {
            var result = new ViewResult<ProductModel>();
            try
            {
                var product = await _productRepository.GetById(id);
                result.Data = product?.ToProductModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "GetById", ex);
            }
            return result;
        }

        /// <summary>
        /// Add or update a product model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<Operate> AddOrUpdate(ProductModel model, string userName)
        {
            var result = new Operate();
            try
            {
                //check duplicated name
                var dupProduct = await _productRepository.GetProductByName(model.Code, model.Id);
                if (dupProduct != null)
                {
                    result.Status = -2;
                    result.Message = "Code is already taken";
                    return result;
                }
                Product product;
                if (model.Id == 0)
                {
                    product = model.ToProductModel();
                    product.CreateTime = DateTime.UtcNow;
                    product.CreateBy = userName;
                }
                else
                {
                    product = await _productRepository.GetById(model.Id);
                    if (product == null)
                    {
                        result.Status = -3;
                        result.Message = "Product does not exist";
                        Logger.WriteErrorLog("ProductService", "AddOrUpdate", new Exception("Product is not existed"));
                        return result;
                    }
                    product.EditTime = DateTime.UtcNow;
                    product.EditBy = userName;
                    product.Vendor = model.Vendor;
                    product.Code = model.Code;
                    product.Image = model.Image;
                    product.LongDesc = model.LongDesc;
                    product.ShortDesc = model.ShortDesc;
                    product.UomId = model.UomId;
                    product.CategoryId = model.CategoryId;
                }
                await _productRepository.AddOrUpdate(product);
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "AddOrUpdate", ex);
            }
            return result;
        }

        /// <summary>
        /// Delete product model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Operate> DeleteById(int id)
        {
            var result = new Operate();
            try
            {
                var product = await _productRepository.GetById(id);
                await _productRepository.Delete(product);
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("ProductService", "DeleteById", ex);
            }
            return result;
        }
    }
}
