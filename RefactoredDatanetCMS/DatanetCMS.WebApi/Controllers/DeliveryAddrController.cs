using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.DeliveryAddrModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class DeliveryAddrController : ApiController
    {
        private readonly DeliveryAddrService _deliveryAddrService;

        public DeliveryAddrController()
        {
            _deliveryAddrService = new DeliveryAddrService();
        }

        [HttpGet]
        public async Task<MulitViewResult<DeliveryAddrModel>> GetAll()
        {
            return await _deliveryAddrService.GetAll();
        }

        [HttpGet]
        public async Task<MulitViewResult<DeliveryAddrModel>> GetAddrsByCustomerId(int id)
        {
            return await _deliveryAddrService.GetAddrsByCustomerId(id);
        }

        [HttpGet]
        public async Task<ViewResult<DeliveryAddrModel>> GetById(int id)
        {
            return await _deliveryAddrService.GetById(id);
        }

        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _deliveryAddrService.DeleteById(id);
        }

        [HttpPost]
        public async Task<ViewResult<DeliveryAddrModel>> AddOrUpdate(DeliveryAddrModel model)
        {
            var userName = User.Identity.Name;
            return await _deliveryAddrService.AddOrUpdate(model, userName);
        }

        [HttpPost]
        public async Task<Operate> addBulkAddress(DeliveryAddrBulkModel model)
        {
            var userName = User.Identity.Name;
            return await _deliveryAddrService.addBulkAddress(model, userName);
        }

        [HttpGet]
        public async Task<ViewResult<PageModel<DeliveryAddrModel>>> Search(int? page, int? pageSize, int customerId, string filter = null)
        {
            return await _deliveryAddrService.Search(page, pageSize,customerId, filter);
        }
    }
}