using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DatanetCMS.Common;
using DatanetCMS.Common.Log;
using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.OrderModel;
using DatanetCMS.Repository;
using System.Text.RegularExpressions;
using System.Threading;
using DatanetCMS.Model.CustomerModel;
using DatanetCMS.Model.PdfModel;
using Hangfire;

namespace DatanetCMS.Service
{
    public class OrderService
    {
        private readonly OrderRepositroy _orderRepositroy;
        private readonly OrderNumberRepository _orderNumberRepository;
        private readonly OrderHistoryRepository _orderHistoryRepository;
        private readonly OrderAddrRepository _orderAddrRepository;
        private readonly OrderProductRepository _orderProductRepository;
        private readonly OrderDocRepository _orderDocRepository;
        private readonly DeliveryAddressRepository _deliveryAddressRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly ManagerRepository _managerRepository;
        private readonly SettingService _settingService;
        public OrderService()
        {
            _orderRepositroy = new OrderRepositroy();
            _orderNumberRepository = new OrderNumberRepository();
            _orderHistoryRepository = new OrderHistoryRepository();
            _orderAddrRepository = new OrderAddrRepository();
            _orderProductRepository = new OrderProductRepository();
            _orderDocRepository = new OrderDocRepository();
            _deliveryAddressRepository = new DeliveryAddressRepository();
            _customerRepository = new CustomerRepository();
            _productRepository = new ProductRepository();
            _managerRepository = new ManagerRepository();
            _settingService = new SettingService();
        }

        ///// <summary>
        ///// get orders or quotes information by mode for customer
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="mode"></param>
        ///// <returns></returns>
        //public async Task<MulitViewResult<OrderModel>> GetOrdersByCustomerId(int id, string mode)
        //{
        //    var result = new MulitViewResult<OrderModel>();

        //    try
        //    {
        //        var customer = await _customerRepository.GetById(id);
        //        if (customer == null)
        //        {
        //            result.Status = -1;
        //            result.Message = "Customer is not exist";
        //            return result;
        //        }

        //        var model = await _orderRepositroy.GetOrdersByCustomerId(id, mode, null);
        //        result.Datas = model
        //            .Select(x => x.ToQuoteModel())
        //            .OrderByDescending(x => x.CreateTime)
        //            .ToList();
        //        result.AllCount = model?.Count ?? 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Status = -1;
        //        result.Message = ex.Message;
        //        Logger.WriteErrorLog("OrderService", "GetOrdersByCustomerId", ex);
        //    }

        //    return result;
        //}
        /// <summary>
        /// get order and quote by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<OrderModel>> GetOrderOrQuoteById(int id)
        {
            var result = new ViewResult<OrderModel>();

            try
            {
                var model = await _orderRepositroy.GetOrderOrQuoteById(id);
                result.Data = model.ToOrderModel();
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "GetOrderOrQuoteById", ex);
            }

            return result;
        }
        /// <summary>
        /// product pagination and show detail 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<PageSingleModel<OrderModel>>> GetOrderOrQuoteProduct(int? page, int? pageSize, int id)
        {
            var result = new ViewResult<PageSingleModel<OrderModel>>();
            try
            {
                var orderOrQuote = await _orderRepositroy.GetOrderOrQuoteById(id);
                var orderAdd = await _orderAddrRepository.GetOrderAddrByOrderId(id);
                var poDoc = await _orderDocRepository.GetOrderDocByOrderId(id);
                if (orderAdd != null)
                {
                    orderOrQuote.OrderAddresses.Clear();
                    orderOrQuote.OrderAddresses.Add(orderAdd);
                }
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                OrderModel orderModels = null;
                int totalOrderProductModels;
                orderModels = orderOrQuote.ToOrderModel();
                if (poDoc != null)
                {
                    orderModels.PoDocPath = poDoc.PoDocPath;
                }
                totalOrderProductModels = orderModels.OrderProductModels.Count();
                if (page == -1 && pageSize == -1)
                {
                    orderModels.OrderProductModels = orderModels.OrderProductModels.ToList();
                }
                else
                {
                    orderModels.OrderProductModels = orderModels.OrderProductModels.Select(x => x).Skip(currentPage * currentPageSize).Take(currentPageSize).ToList();
                }

                result.Data = new PageSingleModel<OrderModel>
                {
                    Page = currentPage,
                    TotalCount = totalOrderProductModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalOrderProductModels / currentPageSize),
                    Item = orderModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "GetOrderOrQuoteProduct", ex);
            }
            return result;
        }

        /// <summary>
        /// product pagination and show detail with image data
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ViewResult<PageModel<OrderProductModel>>> GetOrderOrQuoteProductWithImage(int? page, int? pageSize, int id)
        {
            var result = new ViewResult<PageModel<OrderProductModel>>();
            try
            {
                var orderOrQuote = await _orderRepositroy.GetOrderOrQuoteById(id);
                //var orderAdd = await _orderAddrRepository.GetOrderAddrByOrderId(id);
                //if (orderAdd != null)
                //{
                //    orderOrQuote.OrderAddresses.Clear();
                //    orderOrQuote.OrderAddresses.Add(orderAdd);
                //}
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;

                int totalOrderProductModels;
                var orderModels = orderOrQuote.OrderProducts.Select(x=>x.ToOrderProductModelWithImage());
                totalOrderProductModels = orderModels.Count();
                if (page == -1 && pageSize == -1)
                {
                    orderModels = orderModels.ToList();
                }
                else
                {
                    orderModels = orderModels.Select(x => x).Skip(currentPage * currentPageSize).Take(currentPageSize).ToList();
                }

                result.Data = new PageModel<OrderProductModel>
                {
                    Page = currentPage,
                    TotalCount = totalOrderProductModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalOrderProductModels / currentPageSize),
                    Items = orderModels.ToList()
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "GetOrderOrQuoteProductWithImage", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageModel<QuoteOrderModel>>> Search(FilterModel filterModel)
        {
            var result = new ViewResult<PageModel<QuoteOrderModel>>();
            try
            {
                var orders = await _orderRepositroy.GetAllByFilter(filterModel);
                int currentPage = filterModel.page.Value;
                int currentPageSize = filterModel.pageSize.Value;
                List<QuoteOrderModel> orderModels = null;
                orderModels =
                        orders.Select(x => x.ToQuoteOrderModel())
                            .OrderByDescending(x => x.CreateTime)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                var totalOrderrModels = orders.Count();
                result.Data = new PageModel<QuoteOrderModel>
                {
                    Page = currentPage,
                    TotalCount = totalOrderrModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalOrderrModels / currentPageSize),
                    Items = orderModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "Search", ex);
            }
            return result;
        }

        /// <summary>
        /// add order
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Operate> AddOrder(OrderInfoModel model, string name, string rootUrl)
        {
            var result = new Operate();
            var orderId = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (string.IsNullOrWhiteSpace(model.ContactEmail))
                    {
                        result.Status = -1;
                        result.Message = "Email address is required";
                        return result;
                    }

                    if (!EmailHelper.IsValidEmail(model.ContactEmail))
                    {
                        result.Status = -1;
                        result.Message = "Email address is not valid";
                        return result;
                    }

                    //get customer info
                    var customerId = model.CustomerId;
                    var customer = await _customerRepository.GetById(customerId);
                    if (customer == null)
                    {
                        result.Status = -3;
                        result.Message = "User does not exist";
                        return result;
                    }
                    if (customer.ManagerId==0 || customer.ManagerId == null || customer.Manager == null)
                    {
                        result.Status = -4;
                        result.Message = "Manager needed,PLease contact your adminstrator";
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(model.PurchaseCode))
                    {
                        result.Status = -1;
                        result.Message = "Purchase Order number is empty";
                        return result;
                    }

                    var orderModel = model.ToOrderModel();

                    //purchase code or contract number
                    orderModel.PurchaseType = customer.PurchaseType;
                    orderModel.PurchaseNumber = model.PurchaseCode;

                    //set order id
                    orderModel.Id = 0;
                    orderModel.ContactEmail = model.ContactEmail;

                    orderModel.Mode = ((int)(Constants.OrderMode.Order)).ToString();

                    //create quote code
                    var orderNumber = await _orderNumberRepository.GetOrderNumberByMode(((int)(Constants.OrderMode.Order)).ToString());
                    var orderCode = OrderCodeHelper.GenerateOrderCode(orderNumber.Prefix, orderNumber.CurrentNumber ?? 0);

                    //set quote code
                    orderModel.OrderCode = orderCode;

                    //save order
                    var order = orderModel.ToOrderModel();
                    order.CreateBy = name;
                    order.CreateTime = DateTime.UtcNow;

                    order.CompanyName = customer.CompanyName;
                    order.CustomerContactName = customer.ContactName;
                    order.ManagerId = customer.ManagerId;
                    order.ManagerName = customer.Manager?.Name;

                    order.CreateTime = DateTime.UtcNow;
                    order.CompanyName = customer.CompanyName;
                    order.CustomerContactName = customer.ContactName;
                    order.ManagerId = customer.ManagerId;
                    order.ManagerName = customer.Manager?.Name;
                    var orderAddr = model.ToOrderAddrModel();
                    await _orderRepositroy.AddOrUpdate(order);
                    orderId = order.Id;


                    orderAddr.Id = 0;
                    orderAddr.OrderId = order.Id;
                    orderAddr.CreateBy = name;
                    orderAddr.CreateTime = DateTime.UtcNow;
                    await _orderAddrRepository.AddOrUpdate(orderAddr);
                    //add order product
                    var orderProducts = model.ToOrderProductsModel();
                    if (orderProducts == null || orderProducts.Count <= 0)
                    {
                        result.Status = -1;
                        result.Message = Constants.ErrorOrderHasNoProduct;
                        return result;
                    }
                    foreach (var product in orderProducts)
                    {
                        product.Id = 0;
                        product.OrderId = order.Id;
                        product.CreateBy = name;
                        product.CreateTime = DateTime.UtcNow;
                        await _orderProductRepository.AddOrUpdate(product);
                    }

                    //add order document
                    if (customer.PoDocMandatory ?? false)
                    {
                        var orderDoc = model.ToOrderDocModel();
                        if (orderDoc == null)
                        {
                            result.Status = -1;
                            result.Message = "PO Document is required";
                            return result;
                        }
                        orderDoc.Id = 0;
                        orderDoc.OrderId = order.Id;
                        orderDoc.CreateBy = name;
                        orderDoc.CreateTime = DateTime.UtcNow;
                        await _orderDocRepository.AddOrUpdate(orderDoc);
                    }

                    //add order history
                    var history = new OrderHistory
                    {
                        Id = 0,
                        CreateTime = DateTime.UtcNow,
                        CreateBy = name,
                        TableName = "",
                        FieldName = "",
                        OldValue = "",
                        NewValue = "",
                        Note = Constants.OrderHistoryAddOrder,
                        Operator = Constants.OrderHistoryAddOperator,
                        OrderId = order.Id
                    };
                    await _orderHistoryRepository.AddOrUpdate(history);
                    //await SendEmailById(order.Id, rootUrl, "sendOrder");
                    scope.Complete();
                    BackgroundJob.Enqueue(() => CreateOrderPdfAndSendEmail(rootUrl, customer.ManagerId ?? 0, model, order, name, "sendOrder"));
                }
            }
            catch (Exception ex)
            {
                //add order history
                var history = new OrderHistory
                {
                    Id = 0,
                    CreateTime = DateTime.UtcNow,
                    CreateBy = name,
                    TableName = "",
                    FieldName = "",
                    OldValue = "",
                    NewValue = "",
                    Note = Constants.OrderHistoryAddOrder,
                    Operator = Constants.OrderHistoryErrorOperator,
                    OrderId = orderId
                };
                await _orderHistoryRepository.AddOrUpdate(history);

                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLogForOrder("OrderService", "AddOrder", ex, orderId);
            }
            return result;
        }

        public async Task CreateOrderPdfAndSendEmail(string rootUrl, int managerId, OrderInfoModel model, Order order,string name, string isTripleEmail = null)
        {
            try
            {
                var manager = await _managerRepository.GetById(managerId);
                var orderAddr = model.ToOrderAddrModel();
                var pdfModel = new PdfOrderModel()
                {
                    RootUrl = rootUrl,
                    TemplatePath = System.Configuration.ConfigurationManager.AppSettings["Template_JobOrder"],
                    FileName = string.Format("order{0}", order.OrderCode),
                    HeadContact = "PO Box 1111" + "<br>" + "Burswood WA 6100 " + "<br>" + "T (08) 9470 7300" + "<br>" + "F (08) 9362 6210" + "<br>" + "<br>"
                    + "E accounts@datanet.com.au" + "<br>" + "www.datanet.com.au" + "<br>" + "<br>" + "ABN: 56 123 914 616",
                    HeadDeliverTo = order.ContactName+ (!string.IsNullOrEmpty(order.ContactName) ?"<br>":"") + 
                    order.CompanyName+ (!string.IsNullOrEmpty(order.CompanyName) ? "<br>" : "") + 
                    orderAddr?.Addr1 + (!string.IsNullOrEmpty(orderAddr?.Addr1) ? "<br>" : "") + 
                    orderAddr?.Addr2 + (!string.IsNullOrEmpty(orderAddr?.Addr2) ? "<br>" : "") +
                    orderAddr?.Addr3 + (!string.IsNullOrEmpty(orderAddr?.Addr3) ? "<br>" : "")
                    + orderAddr?.State + "     " + orderAddr?.PostCode,
                    HeadOrderTo = order.CompanyName,
                    HeadOrderDate = order.CreateTime?.AddHours(model.TimeZone).ToString("dd'/'MM'/'yyyy"),
                    HeadRefNumber = order.OrderCode,
                    HeadAccountManager = order.ManagerName + "<br>" + manager?.Phone + "<br>" + "<a href='mailto:"+manager?.Email +"'>"+
                    manager?.Email +"</a>",
                    SumSubTotal = (order.Amount - order.GST)?.ToString("c2", CulturesHelper.Australia),
                    SumGst = order.GST?.ToString("c2", CulturesHelper.Australia),
                    SumAmount = order.Amount?.ToString("c2", CulturesHelper.Australia),
                    OrderDetailList = new List<PdfOrderDetailModel>() { }
                };
                foreach (var productItem in model.OrderProducts)
                {
                    var product = await _productRepository.GetById(productItem.ProductId);
                    pdfModel.OrderDetailList.Add(new PdfOrderDetailModel()
                    {
                        StockCode = product.Code,
                        Description = product.ShortDesc,
                        Qty = productItem.Quantity.ToString() + ".00",
                        UnitPrice = productItem.Price?.ToString("c2", CulturesHelper.Australia),
                        LineTotal = ((productItem.Quantity * productItem.Price)?.ToString("c2", CulturesHelper.Australia))
                    });
                }
                pdfModel.OrderDetailList.Add(new PdfOrderDetailModel()
                {
                    StockCode = "DELIVERY",
                    Description = "Delivery",
                    Qty = "1.00",
                    UnitPrice = model.DeliveryCharge.ToString("c2", CulturesHelper.Australia),
                    LineTotal = model.DeliveryCharge.ToString("c2", CulturesHelper.Australia)
                });
                order.PDFPath = PdfHelper.GetOrderPdfPath(pdfModel);
                await _orderRepositroy.AddOrUpdate(order);
                var sendEmailResult = await SendEmailById(order.Id, rootUrl, isTripleEmail);
                if(sendEmailResult.Status != 0)
                {
                    throw new Exception(sendEmailResult.Message);
                }
            }
            catch(Exception ex)
            {
                //add order history
                var history = new OrderHistory
                {
                    Id = 0,
                    CreateTime = DateTime.UtcNow,
                    CreateBy = name,
                    TableName = "",
                    FieldName = "",
                    OldValue = "",
                    NewValue = "",
                    Note = Constants.OrderHistoryAddOrder,
                    Operator = Constants.OrderHistoryErrorOperator,
                    OrderId = order.Id
                };
                await _orderHistoryRepository.AddOrUpdate(history);

                Logger.WriteErrorLogForOrder("OrderService", "CreateOrderPdfAndSendEmail", ex, order.Id);
            }
        }
        /// <summary>
        /// create order by [name]
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Operate> AddQuote(OrderInfoModel model, string name, string rootUrl)
        {
            var result = new Operate();
            var orderId = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (string.IsNullOrWhiteSpace(model.ContactEmail))
                    {
                        result.Status = -1;
                        result.Message = "Email address is required";
                        return result;
                    }

                    if (!EmailHelper.IsValidEmail(model.ContactEmail))
                    {
                        result.Status = -1;
                        result.Message = "Email address is not valid";
                        return result;
                    }

                    //get customer info
                    var customerId = model.CustomerId;
                    var customer = await _customerRepository.GetById(customerId);

                    if (customer == null)
                    {
                        result.Status = -3;
                        result.Message = "User does not exist";
                        return result;
                    }

                    if (customer.ManagerId == 0 || customer.ManagerId == null || customer.Manager == null)
                    {
                        result.Status = -4;
                        result.Message = "Manager needed,PLease contact your adminstrator";
                        return result;
                    }

                    var orderModel = model.ToOrderModel();
                    //set order id
                    orderModel.Id = 0;
                    orderModel.ContactEmail = orderModel.ContactEmail;
                    orderModel.ContactName = orderModel.ContactName;
                    orderModel.ContactPhone = orderModel.ContactPhone;

                    orderModel.Mode = ((int)(Constants.OrderMode.Quote)).ToString();

                    //create quote code
                    var quoteNumber = await _orderNumberRepository.GetOrderNumberByMode(((int)(Constants.OrderMode.Quote)).ToString());
                    var quoteCode = OrderCodeHelper.GenerateOrderCode(quoteNumber.Prefix, quoteNumber.CurrentNumber ?? 0);

                    //set quote code
                    orderModel.QuoteCode = quoteCode;

                    //save order
                    var order = orderModel.ToOrderModel();
                    order.CreateBy = name;
                    order.CreateTime = DateTime.UtcNow;

                    order.CompanyName = customer.CompanyName;
                    order.CustomerContactName = customer.ContactName;
                    order.ManagerId = customer.ManagerId;
                    order.ManagerName = customer.Manager?.Name;
                    var orderAddr = model.ToOrderAddrModel();
                    await _orderRepositroy.AddOrUpdate(order);
                    orderId = order.Id;

                    //add order address
                    if (orderAddr != null)
                    {
                        orderAddr.Id = 0;
                        orderAddr.OrderId = order.Id;
                        orderAddr.CreateBy = name;
                        orderAddr.CreateTime = DateTime.UtcNow;
                        await _orderAddrRepository.AddOrUpdate(orderAddr);
                    }
                    //add order product
                    var orderProducts = model.ToOrderProductsModel();
                    if (orderProducts == null || orderProducts.Count <= 0)
                    {
                        result.Status = -1;
                        result.Message = Constants.ErrorOrderHasNoProduct;
                        return result;
                    }
                    foreach (var product in orderProducts)
                    {
                        product.Id = 0;
                        product.OrderId = order.Id;
                        product.CreateBy = name;
                        product.CreateTime = DateTime.UtcNow;
                        await _orderProductRepository.AddOrUpdate(product);
                    }

                    //add order history
                    var history = new OrderHistory
                    {
                        Id = 0,
                        CreateTime = DateTime.UtcNow,
                        CreateBy = name,
                        TableName = "",
                        FieldName = "",
                        OldValue = "",
                        NewValue = "",
                        Note = Constants.OrderHistoryAddQuote,
                        Operator = Constants.OrderHistoryAddOperator,
                        OrderId = order.Id
                    };
                    await _orderHistoryRepository.AddOrUpdate(history);
                    scope.Complete();
                    //await SendEmailById(order.Id, rootUrl, "sendQuote");
                    //create pdf and send email
                    BackgroundJob.Enqueue(()=> CreateQuotePdfAndSendEmail(rootUrl, customer.ManagerId??0,model,order, name, "sendQuote"));
                }

            }
            catch (Exception ex)
            {
                //add order history
                var history = new OrderHistory
                {
                    Id = 0,
                    CreateTime = DateTime.UtcNow,
                    CreateBy = name,
                    TableName = "",
                    FieldName = "",
                    OldValue = "",
                    NewValue = "",
                    Note = Constants.OrderHistoryAddQuote,
                    Operator = Constants.OrderHistoryErrorOperator,
                    OrderId = orderId
                };
                await _orderHistoryRepository.AddOrUpdate(history);

                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLogForOrder("OrderService", "AddQuote", ex, orderId);
            }
            return result;
        }

        public async Task CreateQuotePdfAndSendEmail(string rootUrl, int managerId, OrderInfoModel model, Order order, string name, string isTripleEmail = null)
        {
            try
            {
                var manager = await _managerRepository.GetById(managerId);
                var orderAddr = model.ToOrderAddrModel();
                var pdfModel = new PdfQuoteModel
                {
                    RootUrl = rootUrl,
                    TemplatePath = System.Configuration.ConfigurationManager.AppSettings["Template_JobQuote"],
                    FileName = string.Format("quote{0}", order.QuoteCode),
                    HeadContact = "PO Box 1111" + "<br>" + "Burswood WA 6100 " + "<br>" + "T (08) 9470 7300" + "<br>" + "F (08) 9362 6210" + "<br>" + "<br>"
                    + "E accounts@datanet.com.au" + "<br>" + "www.datanet.com.au" + "<br>" + "<br>" + "ABN: 56 123 914 616",
                    HeadDeliverTo = order.ContactName + (!string.IsNullOrEmpty(order.ContactName) ? "<br>" : "") +
                    order.CompanyName + (!string.IsNullOrEmpty(order.CompanyName) ? "<br>" : "") +
                    orderAddr?.Addr1 + (!string.IsNullOrEmpty(orderAddr?.Addr1) ? "<br>" : "") +
                    orderAddr?.Addr2 + (!string.IsNullOrEmpty(orderAddr?.Addr2) ? "<br>" : "") +
                    orderAddr?.Addr3 + (!string.IsNullOrEmpty(orderAddr?.Addr3) ? "<br>" : "")
                    + orderAddr?.State + "     " + orderAddr?.PostCode,
                    HeadQuoteTo = order.CompanyName,
                    HeadQuoteDate = order.CreateTime?.AddHours(model.TimeZone).ToString("dd'/'MM'/'yyyy"),
                    HeadQuotation = order.QuoteCode,
                    HeadAccountManager = order.ManagerName + "<br>" + manager?.Phone + "<br>" + "<a href='mailto:" + manager?.Email + "'>" +
                    manager?.Email + "</a>",
                    SumSubTotal = (order.Amount - order.GST)?.ToString("c2", CulturesHelper.Australia),
                    SumGst = order.GST?.ToString("c2", CulturesHelper.Australia),
                    SumAmount = order.Amount?.ToString("c2", CulturesHelper.Australia),
                    QuoteDetailList = new List<PdfQuoteDetailModel>() { }
                };
                foreach (var productItem in model.OrderProducts)
                {
                    var product = await _productRepository.GetById(productItem.ProductId);
                    pdfModel.QuoteDetailList.Add(new PdfQuoteDetailModel()
                    {
                        StockCode = product.Code,
                        Description = product.ShortDesc,
                        Qty = productItem.Quantity.ToString() + ".00",
                        UnitPrice = productItem.Price?.ToString("c2", CulturesHelper.Australia),
                        LineTotal = ((productItem.Quantity * productItem.Price)?.ToString("c2", CulturesHelper.Australia))
                    });
                }
                pdfModel.QuoteDetailList.Add(new PdfQuoteDetailModel()
                {
                    StockCode = "DELIVERY",
                    Description = "Delivery",
                    Qty = "1.00",
                    UnitPrice = model.DeliveryCharge.ToString("c2", CulturesHelper.Australia),
                    LineTotal = model.DeliveryCharge.ToString("c2", CulturesHelper.Australia)
                });
                order.PDFPath = PdfHelper.GetQuotePdfPath(pdfModel);
                await _orderRepositroy.AddOrUpdate(order);
                // await SendEmailById(order.Id, rootUrl, isTripleEmail);
                var sendEmailResult = await SendEmailById(order.Id, rootUrl, isTripleEmail);
                if (sendEmailResult.Status != 0)
                {
                    throw new Exception(sendEmailResult.Message);
                }
            }
            catch (Exception ex)
            {
                //add order history
                var history = new OrderHistory
                {
                    Id = 0,
                    CreateTime = DateTime.UtcNow,
                    CreateBy = name,
                    TableName = "",
                    FieldName = "",
                    OldValue = "",
                    NewValue = "",
                    Note = Constants.OrderHistoryAddQuote,
                    Operator = Constants.OrderHistoryErrorOperator,
                    OrderId = order.Id
                };
                await _orderHistoryRepository.AddOrUpdate(history);

                Logger.WriteErrorLogForOrder("OrderService", "CreateQuotePdfAndSendEmail", ex, order.Id);
            }
        }


        /// <summary>
        /// send email for quote
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rootUrl"></param>
        /// <returns></returns>
        public async Task<Operate> SendEmailById(int id, string rootUrl, string isTripleEmail = null)
        {
            var result = new Operate();
            try
            {
                var order = await _orderRepositroy.GetOrderOrQuoteById(id);
                if (order == null)
                {
                    result.Status = -1;
                    result.Message = "Order or quote Id does not exist";
                    return result;
                }
                var orderManager = await _managerRepository.GetById(order.ManagerId ?? 0);
                if (orderManager == null)
                {
                    result.Status = -1;
                    result.Message = "Manager Id does not exist";
                    return result;
                }
                var managers = await _managerRepository.GetAllAdmin();
                var to = new List<string>();
                if (!string.IsNullOrEmpty(isTripleEmail))
                {
                    to = (from manager in managers
                          select manager.Email).ToList();
                    to.Add(orderManager.Email);
                    to.Add(order.ContactEmail);
                }
                else
                {
                    to = (from manager in managers
                          select manager.Email).ToList();
                }

                if(string.IsNullOrWhiteSpace(order.PDFPath))
                {
                    result.Status = -1;
                    result.Message = "PDF file can't be found";
                    return result;
                }

				var emailTemplate = _settingService.GetEmailTemplates();
				//var emailTemplate = new MulitViewResult<Model.SettingModel.EmailTemplateModel>();
				if (emailTemplate.Datas.Count<=0)
                {
                    result.Status = -1;
                    result.Message = "Email template cannot be found";
                    return result;
                }
                var subject =emailTemplate.Datas[0]?.Subject;
                var body = emailTemplate.Datas[0]?.Body;
                body = body.Replace("\n", "<br/>");
                var fileList = new List<string> { order.PDFPath };
                new MailService().SendMail(to, new List<string>(), subject, body, fileList);
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "SendEmailById", ex);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// modify quote's IsBuildOrder to true, and add a new order record
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Operate> AddQuoteToOrder(int orderId, OrderInfoModel model, string name,string rootUrl)
        {
            var result = new Operate();
            var orderIdForLog = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (string.IsNullOrWhiteSpace(model.ContactEmail))
                    {
                        result.Status = -1;
                        result.Message = "Email address is required";
                        return result;
                    }

                    if (!EmailHelper.IsValidEmail(model.ContactEmail))
                    {
                        result.Status = -1;
                        result.Message = "Email address is not valid";
                        return result;
                    }

                    //get customer info
                    var customerId = model.CustomerId;
                    var customer = await _customerRepository.GetById(customerId);

                    if (customer == null)
                    {
                        result.Status = -1;
                        result.Message = "User does not exist";
                        return result;
                    }

                    var quote = await _orderRepositroy.GetOrderById(orderId);
                    //no quote
                    if (quote == null)
                    {
                        result.Status = -1;
                        result.Message = "Quote does not exist";
                        return result;
                    }
                    //if build order
                    if (quote.IsBuildOrder ?? false)
                    {
                        result.Status = -1;
                        result.Message = "Quote has been placed order";
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(model.PurchaseCode))
                    {
                        result.Status = -1;
                        result.Message = "Purchase Order number is empty";
                        return result;
                    }

                    //change is build order for quote
                    quote.IsBuildOrder = true;

                    quote.EditTime = DateTime.UtcNow;
                    quote.EditBy = name;

                    await _orderRepositroy.AddOrUpdate(quote);

                    var orderModel = model.ToOrderModel();

                    //purchase code or contract number
                    orderModel.PurchaseType = customer.PurchaseType;
                    orderModel.PurchaseNumber = model.PurchaseCode;

                    //set order id
                    orderModel.Id = 0;
                    orderModel.ContactEmail = model.ContactEmail;

                    orderModel.Mode = ((int)(Constants.OrderMode.Order)).ToString();

                    //create quote code
                    var orderNumber = await _orderNumberRepository.GetOrderNumberByMode(((int)(Constants.OrderMode.Order)).ToString());
                    var orderCode = OrderCodeHelper.GenerateOrderCode(orderNumber.Prefix, orderNumber.CurrentNumber ?? 0);

                    //set order code
                    orderModel.OrderCode = orderCode;
                    //set quote code
                    orderModel.QuoteCode = quote.QuoteCode;
                    //save order
                    var order = orderModel.ToOrderModel();
                    order.CreateBy = name;
                    order.CreateTime = DateTime.UtcNow;

                    order.CompanyName = customer.CompanyName;
                    order.CustomerContactName = customer.ContactName;
                    order.ManagerId = customer.ManagerId;
                    order.ManagerName = customer.Manager?.Name;

                    await _orderRepositroy.AddOrUpdate(order);
                    orderIdForLog = order.Id; 

                    //add order address
                    var orderAddr = model.ToOrderAddrModel();
                    if (orderAddr == null)
                    {
                        result.Status = -1;
                        result.Message = "Delivery address is reqired";
                        return result;
                    }

                    orderAddr.Id = 0;
                    orderAddr.OrderId = order.Id;
                    orderAddr.CreateBy = name;
                    orderAddr.CreateTime = DateTime.UtcNow;
                    await _orderAddrRepository.AddOrUpdate(orderAddr);

                    //add order product
                    var orderProducts = model.ToOrderProductsModel();
                    if (orderProducts == null || orderProducts.Count <= 0)
                    {
                        result.Status = -1;
                        result.Message = Constants.ErrorOrderHasNoProduct;
                        return result;
                    }
                    foreach (var product in orderProducts)
                    {
                        product.Id = 0;
                        product.OrderId = order.Id;
                        product.CreateBy = name;
                        product.CreateTime = DateTime.UtcNow;
                        await _orderProductRepository.AddOrUpdate(product);
                    }


                    //add order document
                    if (customer.PoDocMandatory ?? false)
                    {
                        var orderDoc = model.ToOrderDocModel();
                        if (orderDoc == null)
                        {
                            result.Status = -1;
                            result.Message = "PO Doc is required";
                            return result;
                        }
                        orderDoc.Id = 0;
                        orderDoc.OrderId = order.Id;
                        orderDoc.CreateBy = name;
                        orderDoc.CreateTime = DateTime.UtcNow;
                        await _orderDocRepository.AddOrUpdate(orderDoc);
                    }

                    //add order history
                    var history = new OrderHistory
                    {
                        Id = 0,
                        CreateTime = DateTime.UtcNow,
                        CreateBy = name,
                        TableName = "Order",
                        FieldName = "",
                        OldValue = quote.Id.ToString(),
                        NewValue = order.Id.ToString(),
                        Note = Constants.OrderHistoryQuoteToOrder,
                        Operator = Constants.OrderHistoryAddOperator,
                        OrderId = order.Id
                    };
                    await _orderHistoryRepository.AddOrUpdate(history);
                    //TODO: SEND EMAIL OR OTHER TASK
                    scope.Complete();
                    BackgroundJob.Enqueue(() => CreateOrderPdfAndSendEmail(rootUrl, customer.ManagerId ?? 0, model, order, name, "sendOrder"));
                }
            }
            catch (Exception ex)
            {
                //add order history
                var history = new OrderHistory
                {
                    Id = 0,
                    CreateTime = DateTime.UtcNow,
                    CreateBy = name,
                    TableName = "",
                    FieldName = "",
                    OldValue = "",
                    NewValue = "",
                    Note = Constants.OrderHistoryAddQuote,
                    Operator = Constants.OrderHistoryErrorOperator,
                    OrderId = orderIdForLog
                };
                await _orderHistoryRepository.AddOrUpdate(history);

                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLogForOrder("OrderService", "AddQuoteToOrder", ex, orderIdForLog);
            }
            return result;
        }

        /// <summary>
        /// get deleivery address by customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MulitViewResult<OrderDevliveryAddrModel>> GetDeliveryAddrsByCustomerId(int id)
        {
            var result = new MulitViewResult<OrderDevliveryAddrModel>();
            try
            {
                //get customer all delivery address
                var deliveryAddrs = await _deliveryAddressRepository.GetAddrsByCustomerId(id);
                //get customer order delivery address
                var orderDeliveryAddrs = await _orderAddrRepository.GetOrderAddrByCustomerId(id);

                //customer has no delivery address
                if (deliveryAddrs == null || deliveryAddrs.Count < 1)
                {
                    result.Status = -2;
                    result.Message = "Please contact admin to configure address";
                    return result;
                }

                var deliveryAddrsModel = deliveryAddrs.Select(x => x.ToOrderDevliveryAddrModel()).ToList();

                //customer has no order address
                if (orderDeliveryAddrs == null || orderDeliveryAddrs.Count < 1)
                {
                    result.Datas = deliveryAddrsModel;
                    return result;
                }

                //need to caculate usage rate
                foreach (var orderAddress in orderDeliveryAddrs)
                {
                    var deliveryAddrModel = deliveryAddrsModel.FirstOrDefault(x => x.Id == orderAddress.AddrId);
                    if (deliveryAddrModel == null) continue;
                    deliveryAddrModel.Count++;
                }

                result.Datas = deliveryAddrsModel.OrderByDescending(x => x.Count).ToList();

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "GetDeliveryAddrsByCustomerId", ex);
            }
            return result;
        }

        public async Task<ViewResult<PageModel<CustomerOrderModel>>> SearchOrdersByCustomerId(int? page, int? pageSize, int id, string code, string mode, string dateFilter)
        {
            var result = new ViewResult<PageModel<CustomerOrderModel>>();
            try
            {
                var orders = await _orderRepositroy.GetOrdersByCustomerId(id, mode, code, dateFilter);
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                List<CustomerOrderModel> orderModels = null;
                if (page == -1 && pageSize == -1)
                {
                    orderModels = orders.Select(x => x.ToCustomerOrderModel()).OrderByDescending(x => x.CreateTime).ToList();
                }
                else
                {
                    orderModels =
                        orders.Select(x => x.ToCustomerOrderModel())
                            .OrderByDescending(x => x.CreateTime)
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                }
                var totalOrderrModels = orders.Count();
                result.Data = new PageModel<CustomerOrderModel>
                {
                    Page = currentPage,
                    TotalCount = totalOrderrModels,
                    TotalPages = (int)Math.Ceiling((decimal)totalOrderrModels / currentPageSize),
                    Items = orderModels
                };
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("OrderService", "SearchQuoteByCustomerId", ex);
            }
            return result;
        }
        /// <summary>
        /// check if there have content for order(77) and quote(99)
        /// </summary>
        /// <returns></returns>
        public async Task CheckContent()
        {
            var orderNumber = await _orderNumberRepository.GetOrderNumberByMode(((int)(Constants.OrderMode.Order)).ToString());
            if (orderNumber == null)
            {
                var orderNo = new OrderNumber
                {
                    CurrentNumber = 0,
                    Id = 0,
                    Mode = "7",
                    Prefix = "77"
                };
                await _orderNumberRepository.AddOrUpdate(orderNo);
            }

            var quoteNumber =
                await _orderNumberRepository.GetOrderNumberByMode(((int) (Constants.OrderMode.Quote)).ToString());
            if (quoteNumber == null)
            {
                var quoteNo = new OrderNumber()
                {
                    Id = 0,
                    CurrentNumber = 0,
                    Mode = "9",
                    Prefix = "99"
                };
                await _orderNumberRepository.AddOrUpdate(quoteNo);
            } 
        }
    }
}
