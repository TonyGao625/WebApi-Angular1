using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using DatanetCMS.Common;

namespace DatanetCMS.Service
{
    public class MailService
    {
        public void SendMail(List<string> toList, List<string> ccList, string subject, string body, List<string> fileList)
        {
            const MailPriority priority = MailPriority.Normal;
            var from = ConfigurationManager.AppSettings["FromMail"];
            var smtp = ConfigurationManager.AppSettings["SmtpServer"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var isBodyHtml = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBodyHtml"]);
            EmailHelper.SendMail(from, toList, ccList, subject, body, priority, smtp, port, username, password, isBodyHtml, fileList);
        }
    }
}
