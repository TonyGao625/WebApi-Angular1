using System;
using System.Collections.Generic;
using System.Web.Http;
using DatanetCMS.Model.PdfModel;

namespace DatanetCMS.WebApi.Controllers
{
    /// <summary>
    /// Just for test webapi
    /// </summary>
    public class TestController : ApiController
    {
        public TestController()
        {
            
        }

        [Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        public string Test()
        {
            return "Hello, world! Just for test";
        }

        [HttpGet]
        public string TestSendMail()
        {
            var pdfPath = TestGetQuotePdfPath();
            if (string.IsNullOrEmpty(pdfPath)) return "Pdf generation failed, mail sending NOT start";

            var to = new List<string> { "784627606@qq.com" };
            var subject = string.Format("Test mail subject - {0}", DateTime.UtcNow);
            var body = "<h3>Test H3 Mail Body</h3>";
            var ccList = new List<string> { "gaoqiang@shinetechchina.com" };
            var fileList = new List<string> { pdfPath };
            new Service.MailService().SendMail(to, ccList, subject, body, fileList);
            return "Mail send successed";
        }
        
        [HttpGet]
        public string TestGetQuotePdfPath()
        {
            var model = new PdfQuoteModel
            {
                RootUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority),
                TemplatePath = System.Configuration.ConfigurationManager.AppSettings["Template_JobQuote"],
                FileName = string.Format("jobquote{0:mmssfffff}", DateTime.UtcNow),
                HeadContact = "HeadContact",
                HeadDeliverTo = "HeadDeliverTo11",
                HeadQuoteTo = "HeadQuoteTo111",
                HeadQuoteDate = "HeadQuoteDate1111",
                HeadQuotation = "HeadQuotation111",
                HeadAccountManager = "HeadAccountManager111",
                SumSubTotal = "$55500.50",
                SumGst = "$10000.00",
                SumAmount = "$65500.50",
                QuoteDetailList = new List<PdfQuoteDetailModel>
                {
                    new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
                    new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
                    new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
                    new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
                    new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

                    new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
                    new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
                    new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
                    new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
                    new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

                    new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
                    new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
                    new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
                    new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
                    new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Page1-End", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					/* Page1 = 26 */

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Page2-End", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					/* Page2 = 38 */

					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},
				}
			};
            return Common.PdfHelper.GetQuotePdfPath(model);
        }
		
		[HttpGet]
		public string TestGetOrderPdfPath()
		{
			var model = new PdfOrderModel
			{
				RootUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority),
				TemplatePath = System.Configuration.ConfigurationManager.AppSettings["Template_JobOrder"],
				FileName = string.Format("joborder{0:mmssfffff}", DateTime.Now),
				HeadContact = "HeadContact",
				HeadDeliverTo = "HeadDeliverTo",
				HeadOrderTo = "HeadOrderTo",
				HeadOrderDate = "HeadOrderDate",
				HeadRefNumber = "HeadOrderNumber",
				HeadAccountManager = "HeadAccountManager",
				SumSubTotal = "$555.50",
				SumGst = "$100.00",
				SumAmount = "$655.50",
				OrderDetailList = new List<PdfOrderDetailModel>
				{
					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Page1-End", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					/* Page1 = 26 */

					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					//new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					//new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					//new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},

					//new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					//new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					//new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					//new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					/* Page2 = 38 */

					//new PdfOrderDetailModel{StockCode = "Page2-End", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},
					
					//new PdfOrderDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
					//new PdfOrderDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
					//new PdfOrderDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
					//new PdfOrderDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
					//new PdfOrderDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},
				}
			};
			return Common.PdfHelper.GetOrderPdfPath(model);
		}
	}
}
