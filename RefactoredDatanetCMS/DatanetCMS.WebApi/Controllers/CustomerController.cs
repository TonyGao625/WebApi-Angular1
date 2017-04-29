using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.CustomerModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private readonly CustomerService _customerService;

        public CustomerController()
        {
            _customerService = new CustomerService();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<MulitViewResult<CustomerModel>> GetAll()
        {
            return await _customerService.GetAll();
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<MulitViewResult<CustomerModel>> GetAllName()
        {
            return await _customerService.GetAllName();
        }

        [HttpGet]
        public async Task<ViewResult<CustomerModel>> GetById(int id)
        {
            return await _customerService.GetById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _customerService.DeleteById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ViewResult<CustomerModel>> AddOrUpdate(CustomerModel model)
        {
            var userName = User.Identity.Name;
            return await _customerService.AddOrUpdate(model, userName);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ViewResult<PageModel<CustomerModel>>> Search(int? page, int? pageSize, string filter = null)
        {
            return await _customerService.Search(page, pageSize, filter);
        }

        [HttpGet]
        public async Task<MulitViewResult<CustomerModel>> GetByManagerId(int id)
        {
            return await _customerService.GetByManagerId(id);
        }

    }
}
