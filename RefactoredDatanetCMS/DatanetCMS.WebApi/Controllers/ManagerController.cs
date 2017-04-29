using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.ManagerModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class ManagerController : ApiController
    {
        private readonly ManagerService _managetService;
        public ManagerController()
        {
            _managetService = new ManagerService();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<MulitViewResult<ManagerModel>> GetAll()
        {
            return await _managetService.GetAll();
        }
        [HttpGet]
        public async Task<ViewResult<ManagerModel>> GetById(int id)
        {
            return await _managetService.GetById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _managetService.DeleteById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ViewResult<ManagerModel>> AddOrUpdate(ManagerModel model)
        {
            var userName = User.Identity.Name;
            return await _managetService.AddOrUpdate(model, userName);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ViewResult<PageModel<ManagerModel>>> Search(int? page, int? pageSize, string filter = null)
        {
            return await _managetService.Search(page, pageSize, filter);
        }
    }
}
