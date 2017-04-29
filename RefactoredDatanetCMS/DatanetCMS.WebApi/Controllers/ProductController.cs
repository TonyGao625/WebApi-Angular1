using DatanetCMS.Service;
using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.CategoryModel;
using DatanetCMS.Model.ProductModel;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        private readonly ProductService _productService;

        public ProductController()
        {
            _productService = new ProductService();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<MulitViewResult<ProductModel>> GetAll()
        {
            return await _productService.GetAll();
        }

        [HttpGet]
        public async Task<ViewResult<ProductModel>> GetById(int id)
        {
            return await _productService.GetById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _productService.DeleteById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<Operate> AddOrUpdate(ProductModel model)
        {
            //TODO need to confirm authentication pipeline is used
            var userName = User.Identity.Name;
            return await _productService.AddOrUpdate(model, userName);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ViewResult<PageProductModel>> Search(int? page, int? pageSize, string filter = null)
        {
            return await _productService.Search(page, pageSize, filter);
        }

        [HttpGet]
        public async Task<MulitViewResult<CategoryModel>> GetAllCategoryByCustomerId(int customerId)
        {
            return await _productService.GetAllCategoryByCustomerId(customerId);
        }

        [HttpGet]
        public async Task<ViewResult<PageProductModel>> SearchByCustomerId(int? page, int? pageSize, int customerId, int categoryId,string filter = null)
        {
            return await _productService.SearchByCustomerId(page, pageSize,customerId, categoryId, filter);
        }
    }
}