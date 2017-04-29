using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.CategoryModel;
using DatanetCMS.Model.UomModel;
using DatanetCMS.Repository;

namespace DatanetCMS.Service
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ProductRepository _productRepository;
        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
            _productRepository=new ProductRepository();
        }
        public async Task<MulitViewResult<CategoryModel>> GetAll()
        {
            var result = new MulitViewResult<CategoryModel>();
            try
            {
                var categories = await _categoryRepository.GetAll();
                result.Datas = categories.Select(x => x.ToCategoryModel()).ToList();
                result.AllCount = result.Datas?.Count ?? 0;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CategoryService", "GetAll", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageCategoryModel>> Search(int? page, int? pageSize, string filter = null)
        {
            var result = new ViewResult<PageCategoryModel>();
            try
            {
                var categories = await _categoryRepository.GetAll();
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<CategoryModel> categoryModels = null;
                int totalCategoryModels = new int();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    categoryModels = categories.Select(x => x.ToCategoryModel()).Where(c => c.CategoryName.ToLower().Contains(filter)).OrderBy(c => c.CategoryName)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalCategoryModels = categories.Count(c => c.CategoryName.ToLower().Contains(filter));
                }
                else
                {
                    categoryModels =
                        categories.Select(x => x.ToCategoryModel())
                            .OrderBy(c => c.CategoryName)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                    totalCategoryModels = categories.Count();
                }
                result.Data = new PageCategoryModel()
                {
                    Page = currentPage,
                    TotalCount = totalCategoryModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalCategoryModels / currentPageSize),
                    Items = categoryModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CategoryService", "Search", ex);
            }
            return result;
        }

        public async Task<ViewResult<CategoryModel>> GetById(int id)
        {
            var result = new ViewResult<CategoryModel>();
            try
            {
                var category = await _categoryRepository.GetById(id);
                result.Data = category?.ToCategoryModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CategoryService", "GetById", ex);
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
                    var category = await _categoryRepository.GetById(id);
                    if (category == null)
                    {
                        result.Status = -2;
                        result.Message = "Category does not exist";
                        return result;
                    }
                    var product = await _productRepository.GetProductByCategoryId(id);
                    if (product != null)
                    {
                        result.Status = -3;
                        result.Message = "Category is being used by product";
                        return result;
                    }
                    await _categoryRepository.Delete(category);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CategoryService", "DeleteById", ex);
            }
            return result;
        }

        public async Task<ViewResult<CategoryModel>> AddOrUpdate(CategoryModel model, string userName)
        {
            var result = new ViewResult<CategoryModel>();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //check duplicated name
                    var dupCategory = await _categoryRepository.GetCategoryByName(model.CategoryName, model.Id);
                    if (dupCategory != null)
                    {
                        result.Status = -2;
                        result.Message = "Name is already taken";
                        return result;
                    }
                    Category category;
                    //new uom model
                    if (model.Id == 0)
                    {
                        category=new Category()
                        {
                            CategoryName = model.CategoryName,
                            Id = model.Id,
                            CreateBy = userName,
                            CreateTime = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        category = await _categoryRepository.GetById(model.Id);
                        if (category == null)
                        {
                            result.Status = -3;
                            result.Message = "Category does not exist";
                            return result;
                        }
                        category.EditTime = DateTime.UtcNow;
                        category.EditBy = userName;
                        category.CategoryName = model.CategoryName;
                    }
                    await _categoryRepository.AddOrUpdate(category);
                    result.Data = category.ToCategoryModel();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("CategoryService", "AddOrUpdate", ex);
            }
            return result;
        }
    }
}
