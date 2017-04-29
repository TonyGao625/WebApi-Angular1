using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DatanetCMS.Common;
using DatanetCMS.Model;
using DatanetCMS.Model.OrderModel;
using DatanetCMS.Service;
using DatanetCMS.Common.Log;
using DatanetCMS.Repository;

namespace DatanetCMS.WebApi.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        private readonly OrderService _orderService;
        private readonly CustomerRepository _customerRepository;

        public OrderController()
        {
            _orderService = new OrderService();
            _customerRepository = new CustomerRepository();
        }

        /// <summary>
        /// add order
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Operate> AddOrder(OrderInfoModel model)
        {
            var userName = User.Identity.Name;
            var rootUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return await _orderService.AddOrder(model, userName, rootUrl);
        }

        /// <summary>
        /// add quote
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Operate> AddQuote(OrderInfoModel model)
        {
            var userName = User.Identity.Name;
            var rootUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return await _orderService.AddQuote(model, userName, rootUrl);
        }
        /// <summary>
        /// admin orderquote page send email by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Operate> SendEmailById(int id)
        {
            var rootUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return await _orderService.SendEmailById(id, rootUrl);
        }

        /// <summary>
        /// add quote to order 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Operate> AddQuoteToOrder([FromUri]int id, [FromBody]OrderInfoModel model)
        {
            var userName = User.Identity.Name;
            var rootUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return await _orderService.AddQuoteToOrder(id, model, userName, rootUrl);
        }

        /// <summary>
        /// get delivery addresses according to customer' id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MulitViewResult<OrderDevliveryAddrModel>> GetDeliveryAddrsByCustomerId(int id)
        {
            return await _orderService.GetDeliveryAddrsByCustomerId(id);
        }

        /// <summary>
        /// upload purchase document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ViewResult<UploadModel>> UploadPoDoc()
        {
            var result = new ViewResult<UploadModel>();
            try
            {
                var userName = User.Identity.Name;
                var customer = await _customerRepository.GetByName(userName);
                if (customer == null)
                {
                    result.Status = -1;
                    result.Message = "Can't identify the user";
                    return result;
                }

                if (HttpContext.Current.Request.Files.Count <= 0)
                    return null;

                var file = HttpContext.Current.Request.Files[0];

                //var fileName = Guid.NewGuid() + file.FileName;
                var guidFile = Guid.NewGuid();
                //var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var extensionName = Path.GetExtension(file.FileName);
                var fileName = guidFile + extensionName;

                var path = GetFullPath(fileName, customer.Id);

                file.SaveAs(path);

                result.Data = new UploadModel()
                {
                    FileName = fileName,
                    Path = file.FileName,
                    Size = file.ContentLength
                };
            }
            catch(Exception ex)
            {
                result.Status = -1;
                result.Message = "Upload failed";
                Logger.WriteErrorLog("OrderService", "UploadPoDoc", ex);
            }
            
            return result;
        }
        
        ///// <summary>
        ///// get orders information include products, address for customer
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<MulitViewResult<OrderModel>> GetOrdersByCustomerId(int id)
        //{
        //    return await _orderService.GetOrdersByCustomerId(id, ((int)Constants.OrderMode.Order).ToString());
        //}

        /// <summary>
        /// get quote or order by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ViewResult<OrderModel>> GetOrderOrQuoteById(int id)
        {
            return await _orderService.GetOrderOrQuoteById(id);
        }

        /// <summary>
        /// get product information by order/quote id
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ViewResult<PageSingleModel<OrderModel>>> GetOrderOrQuoteProduct(int? page, int? pageSize,int id)
        {
            return await _orderService.GetOrderOrQuoteProduct(page,pageSize,id);
        }

        /// <summary>
        /// get product information by order/quote id
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ViewResult<PageModel<OrderProductModel>>> GetOrderOrQuoteProductWithImage(int? page, int? pageSize, int id)
        {
            return await _orderService.GetOrderOrQuoteProductWithImage(page, pageSize, id);
        }

        ///// <summary>
        ///// get quotes information include products(product, uom, category), address for customer
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<MulitViewResult<OrderModel>> GetQuotesByCustomerId(int id)
        //{
        //    return await _orderService.GetOrdersByCustomerId(id, ((int)Constants.OrderMode.Quote).ToString());
        //}

        /// <summary>
        /// search all quote and order
        /// used by quote and order tab in admin portal
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ViewResult<PageModel<QuoteOrderModel>>> Search(FilterModel filterModel)
        {
            return await _orderService.Search(filterModel);
        }

        [HttpGet]
        public async Task<ViewResult<PageModel<CustomerOrderModel>>> SearchQuotesByCustomerId(int?page, int?pageSize, int id, string code, string dateFilter = "0")
        {
            return await _orderService.SearchOrdersByCustomerId(page, pageSize, id, code, ((int)Constants.OrderMode.Quote).ToString(), dateFilter);
        }

        [HttpGet]
        public async Task<ViewResult<PageModel<CustomerOrderModel>>> SearchOrdersByCustomerId(int? page, int? pageSize, int id, string code, string dateFilter = "0")
        {
            return await _orderService.SearchOrdersByCustomerId(page, pageSize, id, code, ((int)Constants.OrderMode.Order).ToString(), dateFilter);
        }

        /// <summary>
        /// search manager's quote and order
        /// used by quote and order tab in manager portal
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ViewResult<PageModel<QuoteOrderModel>>> SearchByManagerId(FilterModel filterModel)
        {
            return await _orderService.Search(filterModel);
        }



        private static string GetDirectoryPath(int userId)
        {
            string relativePathBuff = ConfigurationManager.AppSettings["PdfPath"];
            var relativePath = relativePathBuff + @"/" + userId;
            var directory = HttpContext.Current.Server.MapPath(relativePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }

        private static string GetFullPath(string file, int userId)
        {
            var directory = GetDirectoryPath(userId);
            return Path.Combine(directory, file);
        }
    }
}
