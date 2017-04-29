using System.Collections.Generic;

namespace DatanetCMS.Model.PdfModel
{
	#region Quote

	public class PdfQuoteDetailModel
	{
		public string StockCode { get; set; }
		public string Description { get; set; }
		public string Qty { get; set; }
		public string UnitPrice { get; set; }
		public string LineTotal { get; set; }
	}

	public class PdfQuoteModel
	{
		public PdfQuoteModel()
		{
			QuoteDetailList = new List<PdfQuoteDetailModel>();
		}

		public string RootUrl { get; set; }
		public string TemplatePath { get; set; }
		public string ReportPath { get; set; }
		public string FileName { get; set; }

		public string HeadContact { get; set; }
		public string HeadQuoteDate { get; set; }
		public string HeadQuotation { get; set; }
		public string HeadAccountManager { get; set; }
		public string HeadQuoteTo { get; set; }
		public string HeadDeliverTo { get; set; }
		public List<PdfQuoteDetailModel> QuoteDetailList { get; set; }
		public string SumSubTotal { get; set; }
		public string SumGst { get; set; }
		public string SumAmount { get; set; }
	}

	#endregion
	
	#region Order

	public class PdfOrderDetailModel
	{
		public string StockCode { get; set; }
		public string Description { get; set; }
		public string Qty { get; set; }
		public string UnitPrice { get; set; }
		public string LineTotal { get; set; }
	}

	public class PdfOrderModel
	{
		public PdfOrderModel()
		{
			OrderDetailList = new List<PdfOrderDetailModel>();
		}

		public string RootUrl { get; set; }
		public string TemplatePath { get; set; }
		public string ReportPath { get; set; }
		public string FileName { get; set; }

		public string HeadContact { get; set; }
		public string HeadOrderDate { get; set; }
		public string HeadRefNumber { get; set; }
		public string HeadAccountManager { get; set; }
		public string HeadOrderTo { get; set; }
		public string HeadDeliverTo { get; set; }
		public List<PdfOrderDetailModel> OrderDetailList { get; set; }
		public string SumSubTotal { get; set; }
		public string SumGst { get; set; }
		public string SumAmount { get; set; }
	}

	#endregion

}
