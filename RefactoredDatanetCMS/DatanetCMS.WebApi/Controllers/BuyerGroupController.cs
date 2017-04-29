using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DatanetCMS.Model;
using DatanetCMS.Model.BuyerGroupModel;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BuyerGroupController : ApiController
    {
        private readonly BuyerGroupService _buyerGroupService;

        public BuyerGroupController()
        {
            _buyerGroupService = new BuyerGroupService();
        }
        
        [HttpGet]
        public async Task<MulitViewResult<BuyerGroupModel>> GetAll()
        {
            return await _buyerGroupService.GetAll();
        }
        [HttpGet]
        public async Task<ViewResult<GroupProductModel>> GetById(int id)
        {
            return await _buyerGroupService.GetById(id);
        }

        [HttpGet]
        public async Task<Operate> DeleteById(int id)
        {
            return await _buyerGroupService.DeleteById(id);
        }

        [HttpPost]
        public async Task<Operate> AddOrUpdate(GroupProductModel model)
        {
            var userName = User.Identity.Name;
            return await _buyerGroupService.AddOrUpdate(model, userName);
        }

        [HttpGet]
        public async Task<ViewResult<PageBuyerGroupModel>> Search(int? page, int? pageSize, string filter = null)
        {
            return await _buyerGroupService.Search(page, pageSize, filter);
        }
    }
}
