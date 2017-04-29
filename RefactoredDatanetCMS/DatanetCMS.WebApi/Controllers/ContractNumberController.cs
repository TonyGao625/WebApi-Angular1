using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.ContractNumberModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class ContractNumberController : ApiController
    {
        private readonly ContractNumberService _contractNumberService;

        public ContractNumberController()
        {
            _contractNumberService = new ContractNumberService();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<MulitViewResult<ContractNumberModel>> GetAll()
        {
            return await _contractNumberService.GetAll();
        }

        [HttpGet]
        public async Task<MulitViewResult<ContractNumberModel>> GetByCustomerId(int id)
        {
            return await _contractNumberService.GetByCustomerId(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ViewResult<PageModel<ContractNumberModel>>> Search(int? page, int? pageSize, int customerId, string filter = null)
        {
            return await _contractNumberService.Search(page, pageSize, customerId, filter);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ViewResult<ContractNumberModel>> AddOrUpdate(ContractNumberModel model)
        {
            var userName = User.Identity.Name;
            return await _contractNumberService.AddOrUpdate(model, userName);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _contractNumberService.DeleteById(id);
        }

        [HttpGet]
        public async Task<MulitViewResult<ContractNumberModel>> GetContractsByCustomerId(int id)
        {
            return await _contractNumberService.GetContractsByCustomerId(id);
        }
    }
}