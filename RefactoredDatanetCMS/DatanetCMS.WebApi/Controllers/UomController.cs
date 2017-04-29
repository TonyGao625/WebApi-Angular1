using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.UomModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    /// <summary>
    /// Uom controller
    /// </summary>
    [Authorize]
    public class UomController : ApiController
    {
        private readonly UomService _uomService;

        public UomController()
        {
            _uomService = new UomService();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<MulitViewResult<UomModel>> GetAll()
        {
            return await _uomService.GetAll();
        }

        [HttpGet]
        public async Task<ViewResult<UomModel>> GetById(int id)
        {
            return await _uomService.GetById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _uomService.DeleteById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ViewResult<UomModel>> AddOrUpdate(UomModel model)
        {
            var userName = User.Identity.Name;
            return await _uomService.AddOrUpdate(model, userName);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ViewResult<PageUomModel>> Search(int? page, int? pageSize, string filter = null)
        {
            return await _uomService.Search(page, pageSize, filter);
        }

     
    }
}