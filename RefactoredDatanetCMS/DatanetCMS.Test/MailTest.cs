using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatanetCMS.Service;

namespace DatanetCMS.Test
{
    [TestClass]
    public class MailTest
    {
        private MailService _sendMailClient;

        [TestInitialize]
        public void SendMailInitialize()
        {
            _sendMailClient = new MailService();
        }
        
        [TestMethod]
        public void SendMailTest()
        {
            var to = new List<string> { "jinzesudawei@163.com" };
            var subject = string.Format("Test mail subject - {0}", DateTime.UtcNow);
            var body = "<h3>Test H3 Mail Body</h3>";
            var ccList = new List<string> { "jinzesudawei@qq.com" };
            var fileList = new List<string>();
            _sendMailClient.SendMail(to, ccList, subject, body, fileList);
        }
    }
}
