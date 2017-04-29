using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using SelectPdf;
using DatanetCMS.Model.PdfModel;

namespace DatanetCMS.Common
{
    public class PdfHelper
    {
        private static string MapPath
        {
            get { return System.Web.Hosting.HostingEnvironment.MapPath("~/"); }
        }

        private static string GetReportPath()
        {
            var reportPath = string.Format(@"Reports\{0:yyyy-MM-dd}\{1}", DateTime.UtcNow, Guid.NewGuid());
            var fullPath = string.Format(@"{0}\{1}", MapPath, reportPath);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            return reportPath;
        }

		#region Quote

		private static string GetQuoteHtmlUrl(PdfQuoteModel model)
		{
			var htmlUrl = string.Format(@"{0}/{1}", model.RootUrl, model.TemplatePath);
			var request = (HttpWebRequest)WebRequest.Create(htmlUrl);
			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK) return string.Empty;

			var responseStream = response.GetResponseStream();
			if (responseStream == null) return string.Empty;

			var readStream = response.CharacterSet == null
				? new StreamReader(responseStream)
				: new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet));
			var content = readStream.ReadToEnd();

			/* Value Replacement */
			content = content.Replace("{{head_contact}}", model.HeadContact);
			content = content.Replace("{{head_quote_date}}", model.HeadQuoteDate);
			content = content.Replace("{{head_quotation}}", model.HeadQuotation);
			content = content.Replace("{{head_account_manager}}", model.HeadAccountManager);
			content = content.Replace("{{head_quote_to}}", model.HeadQuoteTo);
			content = content.Replace("{{head_deliver_to}}", model.HeadDeliverTo);
			content = content.Replace("{{sum_sub_total}}", model.SumSubTotal);
			content = content.Replace("{{sum_gst}}", model.SumGst);
			content = content.Replace("{{sum_amount}}", model.SumAmount);
			
			#region Build Quote Table

			var needManualClosed = false;
			var totalCount = model.QuoteDetailList.Count;
			const string rowPageBreak = @"<div class='row'><div style='page-break-before: always'></div></div>";
			const string tableHead = @"<div class='row'><div class='col-xs-12 table_right_angled' style='padding-top: 2px;'>
										<table class='table table-bordered right_angled'>
											<thead>
											<tr>
												<th style='width: 250px;'>STOCKCODE</th>
												<th>DESCRIPTION</th>
												<th style='width: 80px;'>QTY</th>
												<th style='width: 120px;'>UNIT PRICE</th>
												<th style='width: 120px;'>LINE TOTAL</th>
											</tr>
											</thead>
											<tbody>";
			const string tableRow = @"<tr><td>{0}</td><td>{1}</td><td class='text-right'>{2}</td><td class='text-right'>{3}</td><td class='text-right'>{4}</td></tr>";
			const string tableFoot = @"</tbody></table></div></div>";
			var sbDetailList = new StringBuilder(tableHead);
			foreach (var item in model.QuoteDetailList)
			{
				sbDetailList.AppendLine(string.Format(tableRow, item.StockCode, item.Description, item.Qty, item.UnitPrice, item.LineTotal));
				var currentNumber = model.QuoteDetailList.IndexOf(item) + 1;
				var otherPageRemainder = (currentNumber - 26) % 38;

				#region Page Break
				
				/* Bank Panel */
				var pageBreakBank = totalCount >= 20 && totalCount <= 25 || otherPageRemainder >= 32 && otherPageRemainder <= 37 ? rowPageBreak : string.Empty;
				content = content.Replace("{{page_break_bank}}", pageBreakBank);

				/* Terms Panel */
				var pageBreakTerm = totalCount >= 17 && totalCount <= 19 || otherPageRemainder >= 29 && otherPageRemainder <= 31 ? rowPageBreak : string.Empty;
				content = content.Replace("{{page_break_term}}", pageBreakTerm);

				#endregion

				needManualClosed = currentNumber == 26 || otherPageRemainder == 0;
				if (!needManualClosed) continue;

				sbDetailList.AppendLine(tableFoot);
				sbDetailList.AppendLine(rowPageBreak);
				if (currentNumber >= totalCount) continue;

				sbDetailList.AppendLine(tableHead);
			}
			if (!needManualClosed)
			{
				sbDetailList.AppendLine(tableFoot);
			}

			#endregion
			
			content = content.Replace("{{table_details}}", sbDetailList.ToString());
			var htmlPath = string.Format(@"{0}\{1}.html", model.ReportPath, model.FileName);
			var fullPath = string.Format(@"{0}\{1}", MapPath, htmlPath);
			File.WriteAllText(fullPath, content);
			readStream.Close();
			response.Close();

			return string.Format(@"{0}/{1}", model.RootUrl, htmlPath);
		}

		public static string GetQuotePdfPath(PdfQuoteModel model)
		{
			model.ReportPath = GetReportPath();
			var htmlUrl = GetQuoteHtmlUrl(model);
			if (string.IsNullOrEmpty(htmlUrl)) return string.Empty;

			/* page settings */
			var converter = new HtmlToPdf();
			converter.Options.PdfPageSize = PdfPageSize.A4;
			converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
			converter.Options.MarginTop = 24;
			converter.Options.MarginLeft = 10;
			converter.Options.MarginRight = 10;
			converter.Options.MarginBottom = 0;

			/* footer settings */
			converter.Options.DisplayFooter = true;
			converter.Footer.DisplayOnFirstPage = true;
			converter.Footer.DisplayOnOddPages = true;
			converter.Footer.DisplayOnEvenPages = true;
			converter.Footer.Height = 24;
			converter.Footer.Add(new PdfTextSection(0, -10, "Page: {page_number} of {total_pages}", new Font("Arial", 6))
			{
				HorizontalAlign = PdfTextHorizontalAlign.Center
			});

			/* new pdf */
			var pdfPath = string.Format(@"{0}\{1}.pdf", model.ReportPath, model.FileName);
			var fullPath = string.Format(@"{0}\{1}", MapPath, pdfPath);
			var doc = converter.ConvertUrl(htmlUrl);
			doc.Save(fullPath);
			doc.Close();
			return fullPath;
		}

		#endregion

		#region Order

		private static string GetOrderHtmlUrl(PdfOrderModel model)
		{
			var htmlUrl = string.Format(@"{0}/{1}", model.RootUrl, model.TemplatePath);
			var request = (HttpWebRequest)WebRequest.Create(htmlUrl);
			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK) return string.Empty;

			var responseStream = response.GetResponseStream();
			if (responseStream == null) return string.Empty;

			var readStream = response.CharacterSet == null
				? new StreamReader(responseStream)
				: new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet));
			var content = readStream.ReadToEnd();

			/* Value Replacement */
			content = content.Replace("{{head_contact}}", model.HeadContact);
			content = content.Replace("{{head_order_date}}", model.HeadOrderDate);
			content = content.Replace("{{head_ref_number}}", model.HeadRefNumber);
			content = content.Replace("{{head_account_manager}}", model.HeadAccountManager);
			content = content.Replace("{{head_order_to}}", model.HeadOrderTo);
			content = content.Replace("{{head_deliver_to}}", model.HeadDeliverTo);
			content = content.Replace("{{sum_sub_total}}", model.SumSubTotal);
			content = content.Replace("{{sum_gst}}", model.SumGst);
			content = content.Replace("{{sum_amount}}", model.SumAmount);
			
			#region Build Order Table

			var needManualClosed = false;
			var totalCount = model.OrderDetailList.Count;
			const string rowPageBreak = @"<div class='row'><div style='page-break-before: always'></div></div>";
			const string tableHead = @"<div class='row'><div class='col-xs-12 table_right_angled' style='padding-top: 2px;'>
										<table class='table table-bordered right_angled'>
											<thead>
											<tr>
												<th style='width: 250px;'>STOCKCODE</th>
												<th>DESCRIPTION</th>
												<th style='width: 80px;'>QTY</th>
												<th style='width: 120px;'>UNIT PRICE</th>
												<th style='width: 120px;'>LINE TOTAL</th>
											</tr>
											</thead>
											<tbody>";
			const string tableRow = @"<tr><td>{0}</td><td>{1}</td><td class='text-right'>{2}</td><td class='text-right'>{3}</td><td class='text-right'>{4}</td></tr>";
			const string tableFoot = @"</tbody></table></div></div>";
			var sbDetailList = new StringBuilder(tableHead);
			foreach (var item in model.OrderDetailList)
			{
				sbDetailList.AppendLine(string.Format(tableRow, item.StockCode, item.Description, item.Qty, item.UnitPrice, item.LineTotal));
				var currentNumber = model.OrderDetailList.IndexOf(item) + 1;
				var otherPageRemainder = (currentNumber - 26) % 38;

				#region Page Break

				/* Bank Panel */
				var pageBreakBank = totalCount >= 20 && totalCount <= 25 || otherPageRemainder >= 32 && otherPageRemainder <= 37 ? rowPageBreak : string.Empty;
				content = content.Replace("{{page_break_bank}}", pageBreakBank);

				/* Terms Panel */
				var pageBreakTerm = totalCount >= 17 && totalCount <= 19 || otherPageRemainder >= 29 && otherPageRemainder <= 31 ? rowPageBreak : string.Empty;
				content = content.Replace("{{page_break_term}}", pageBreakTerm);

				#endregion

				needManualClosed = currentNumber == 26 || otherPageRemainder == 0;
				if (!needManualClosed) continue;

				sbDetailList.AppendLine(tableFoot);
				sbDetailList.AppendLine(rowPageBreak);
				if (currentNumber >= totalCount) continue;

				sbDetailList.AppendLine(tableHead);
			}
			if (!needManualClosed)
			{
				sbDetailList.AppendLine(tableFoot);
			}

			#endregion
			
			content = content.Replace("{{table_details}}", sbDetailList.ToString());
			var htmlPath = string.Format(@"{0}\{1}.html", model.ReportPath, model.FileName);
			var fullPath = string.Format(@"{0}\{1}", MapPath, htmlPath);
			File.WriteAllText(fullPath, content);
			readStream.Close();
			response.Close();

			return string.Format(@"{0}/{1}", model.RootUrl, htmlPath);
		}

		public static string GetOrderPdfPath(PdfOrderModel model)
		{
			model.ReportPath = GetReportPath();
			var htmlUrl = GetOrderHtmlUrl(model);
			if (string.IsNullOrEmpty(htmlUrl)) return string.Empty;

			/* page settings */
			var converter = new HtmlToPdf();
			converter.Options.PdfPageSize = PdfPageSize.A4;
			converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
			converter.Options.MarginTop = 24;
			converter.Options.MarginLeft = 10;
			converter.Options.MarginRight = 10;
			converter.Options.MarginBottom = 0;

			/* footer settings */
			converter.Options.DisplayFooter = true;
			converter.Footer.DisplayOnFirstPage = true;
			converter.Footer.DisplayOnOddPages = true;
			converter.Footer.DisplayOnEvenPages = true;
			converter.Footer.Height = 24;
			converter.Footer.Add(new PdfTextSection(0, -10, "Page: {page_number} of {total_pages}", new Font("Arial", 6))
			{
				HorizontalAlign = PdfTextHorizontalAlign.Center
			});

			/* new pdf */
			var pdfPath = string.Format(@"{0}\{1}.pdf", model.ReportPath, model.FileName);
			var fullPath = string.Format(@"{0}\{1}", MapPath, pdfPath);
			var doc = converter.ConvertUrl(htmlUrl);
			doc.Save(fullPath);
			doc.Close();
			return fullPath;
		}

		#endregion
	}
}
