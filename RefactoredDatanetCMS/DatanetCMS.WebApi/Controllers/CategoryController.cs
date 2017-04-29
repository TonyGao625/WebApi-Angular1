using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.CategoryModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class CategoryController : ApiController
    {
        private readonly CategoryService _categoryService;

        public CategoryController()
        {
            _categoryService = new CategoryService();
        }
        [HttpGet]
        public async Task<MulitViewResult<CategoryModel>> GetAll()
        {
            return await _categoryService.GetAll();
        }

        [HttpGet]
        public async Task<ViewResult<PageCategoryModel>> Search(int? page, int? pageSize, string filter = null)
        {
            return await _categoryService.Search(page, pageSize, filter);
        }

        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _categoryService.DeleteById(id);
        }

        [HttpGet]
        public async Task<ViewResult<CategoryModel>> GetById(int id)
        {
            return await _categoryService.GetById(id);
        }

        [HttpPost]
        public async Task<ViewResult<CategoryModel>> AddOrUpdate(CategoryModel model)
        {
            var userName = User.Identity.Name;
            return await _categoryService.AddOrUpdate(model, userName);
        }
    }
}
