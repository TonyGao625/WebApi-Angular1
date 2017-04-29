using System.Web.Http;
using System.Web.Http.Cors;
using DatanetCMS.Model.SettingModel;
using DatanetCMS.WebApi.App_Config;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using DatanetCMS.Service;
using Hangfire.MemoryStorage;
using Hangfire;

[assembly: OwinStartup(typeof(DatanetCMS.WebApi.Startup))]

namespace DatanetCMS.WebApi
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        static Startup()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }
        public void Configuration(IAppBuilder app)
        {
            ManagerService managerServcie = new ManagerService();
            managerServcie.CheckAdmin();

            OrderService os = new OrderService();
            os.CheckContent();

            SettingService emailTemplateSetting = new SettingService();
            var emailTemplates = emailTemplateSetting.GetEmailTemplates();
            if (emailTemplates?.Datas == null || emailTemplates.Datas.Count < 1)
            {
                var emailTempalte = new EmailTemplateModel
                {
                    Id = 0,
                    Body = "Dear Customer,\n\nWe recieved you order.\n\nThanks for your booking.\n\nDatanet",
                    Subject = "Thanks for your booking"
                };
                emailTemplateSetting.SaveEmailTemplate(emailTempalte);
            }
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            Hangfire.GlobalConfiguration.Configuration.UseMemoryStorage();
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            HttpConfiguration config = new HttpConfiguration();

            //Cors configuration
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //Cookie configuration
            //app.UseCookieAuthentication( new CookieAuthenticationOptions()
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    CookieName = "DATANETCMS",
            //});
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            WebApiConfig.Register(config);


            app.UseWebApi(config);
        }
    }
}
