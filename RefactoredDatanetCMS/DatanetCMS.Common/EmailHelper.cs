using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DatanetCMS.Common
{
	public class EmailHelper
	{
		public static bool IsValidEmail(string address)
		{
			try
			{
				var result = new MailAddress(address);
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		#region Send Email

		public static void SendMail(string from, List<string> toList, List<string> ccList, string subject, string body,
			MailPriority priority, string smtp, int port, string username, string password, bool isBodyHtml, List<string> fileList)
		{
			using (var mail = new MailMessage())
			{
				mail.Priority = priority;
				mail.IsBodyHtml = isBodyHtml;
				mail.Subject = subject;
				mail.Body = body;
				mail.BodyEncoding = Encoding.UTF8;
				mail.SubjectEncoding = Encoding.UTF8;
				mail.From = new MailAddress(from);
				if (toList.Any())
					foreach (var item in toList)
						mail.To.Add(new MailAddress(item));
				if (ccList.Any())
					foreach (var item in ccList)
						mail.CC.Add(new MailAddress(item));
				if (fileList.Any())
					foreach (var attachment in fileList.Select(t => new Attachment(t)))
						mail.Attachments.Add(attachment);
				using (var context = new SmtpClient(smtp, port))
				{
					context.Credentials = new NetworkCredential(username, password);
					context.Send(mail);
				}
			}
		}

		#endregion
	}
}