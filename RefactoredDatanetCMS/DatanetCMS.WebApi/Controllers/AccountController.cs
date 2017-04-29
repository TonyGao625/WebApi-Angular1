using System;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using DatanetCMS.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace DatanetCMS.WebApi.Controllers
{
    /// <summary>
    /// Account management
    /// </summary>
    public class AccountController : ApiController
    {
        private readonly AccountService _accountService;

        public AccountController()
        {
            _accountService = new AccountService();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JObject> Login(JObject user)
        {
            var userName = user["userName"].ToString();
            var userPwd = user["userPwd"].ToString();

            var result = await _accountService.Login(userName, userPwd);
            if (result.Status != 0)
            {
                return new JObject()
                {
                    ["Status"] = result.Status,
                    ["Message"] = result.Message
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Data.Name),
                new Claim(ClaimTypes.Role, result.Data.Role)
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ticket = new AuthenticationTicket(identity, CreateProperties(userName, result.Data.Role));
            ticket.Properties.IssuedUtc = DateTime.UtcNow;
            ticket.Properties.ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1));
            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            //var owinContext = Request.GetOwinContext();
            //var authManager = owinContext.Authentication;
            //authManager.SignIn(identity);

            var userInfo = new JObject();
            userInfo["Id"] = result.Data.Id;
            userInfo["Name"] = result.Data.Name;
            userInfo["Role"] = result.Data.Role;
            userInfo["Logo"] = result.Data.Logo;

            if(result.Data.Role == "Customer")
            {
                userInfo["CompanyName"] = result.Data.Customer.CompanyName;
                userInfo["ContactName"] = result.Data.Customer.ContactName;
                userInfo["ContactPhone"] = result.Data.Customer.ContactPhone;
                userInfo["ContactEmail"] = result.Data.Customer.ContactEmail;
                userInfo["ManagerId"] = result.Data.Customer.ManagerId.ToString();
                userInfo["PurchaseType"] = result.Data.Customer.PurchaseType;
                userInfo["DisplayContractNo"] = result.Data.Customer.DisplayContractNo;
                
                userInfo["PoDocMandatory"] = result.Data.Customer.PoDocMandatory;
                userInfo["DeliveryCharge"] = result.Data.Customer.DeliveryCharge;
                if(result.Data.Customer.Manager != null)
                {
                    userInfo["ManagerName"] = result.Data.Customer.Manager.Name;
                    userInfo["ManagerEmail"] = result.Data.Customer.Manager.Email;
                    userInfo["ManagerPhone"] = result.Data.Customer.Manager.Phone;
                }
                else
                {
                    userInfo["ManagerName"] = "";
                    userInfo["ManagerEmail"] = "";
                    userInfo["ManagerPhone"] = "";
                }
            }

            return new JObject()
            {
                ["Status"] = 0,
                ["UserInfo"] = userInfo,
                //new JObject {
                //    ["Id"] = result.Data.Id,
                //    ["Name"] = result.Data.Name,
                //    ["Role"] = result.Data.Role,
                //    ["Logo"] = result.Data.Logo
                //},
                ["AccessToken"] = accessToken
            };
        }

        private static AuthenticationProperties CreateProperties(string userName, string userRole)
        {
            var data = new Dictionary<string, string>
                {
                    { "name", userName},
                    { "role", userRole }
                };
            return new AuthenticationProperties(data);
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<Operate> Logout()
        //{
        //    var result = new Operate();
        //    try
        //    {
        //        var name = User.Identity.Name;
        //        var owinContext = Request.GetOwinContext();
        //        var authManager = owinContext.Authentication;
        //        authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Status = -1;
        //        result.Message = "logout error";
        //        Logger.WriteErrorLog("AccountService", "Logout", ex);
        //    }
        //    return result;
        //}
    }
}