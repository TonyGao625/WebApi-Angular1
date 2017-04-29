using System;
using System.Collections.Generic;
using DatanetCMS.Model.PdfModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatanetCMS.Test
{
    [TestClass]
    public class PdfTest
    {
        [TestMethod]
        public void GeneratePdfTest()
        {
            var model = new PdfQuoteModel
            {
                RootUrl = "/",
                TemplatePath = System.Configuration.ConfigurationManager.AppSettings["Template_JobQuote"],
                FileName = string.Format("jobquote{0:mmssfffff}", DateTime.UtcNow),
                HeadContact = "HeadContact",
                HeadDeliverTo = "HeadDeliverTo",
                HeadQuoteTo = "HeadQuoteTo",
                HeadQuoteDate = "HeadQuoteDate",
                HeadQuotation = "HeadQuotation",
                HeadAccountManager = "HeadAccountManager",
                SumSubTotal = "$555.50",
                SumGst = "$100.00",
                SumAmount = "$655.50",
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

                    new PdfQuoteDetailModel{StockCode = "Product-A", Description = "Description-A", Qty = "$10.00", UnitPrice = "$1.01", LineTotal = "$10.10"},
                    new PdfQuoteDetailModel{StockCode = "Product-B", Description = "Description-B", Qty = "$20.00", UnitPrice = "$2.02", LineTotal = "$40.40"},
                    new PdfQuoteDetailModel{StockCode = "Product-C", Description = "Description-C", Qty = "$30.00", UnitPrice = "$3.03", LineTotal = "$90.90"},
                    new PdfQuoteDetailModel{StockCode = "Product-D", Description = "Description-D", Qty = "$40.00", UnitPrice = "$4.04", LineTotal = "$161.60"},
                    new PdfQuoteDetailModel{StockCode = "Product-E", Description = "Description-E", Qty = "$50.00", UnitPrice = "$5.05", LineTotal = "$252.50"},
                }
            };
            var result = Common.PdfHelper.GetQuotePdfPath(model);
            Assert.IsNotNull(result);
        }
    }
}
