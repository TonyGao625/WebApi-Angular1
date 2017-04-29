using DatanetCMS.Model;
using DatanetCMS.Model.SettingModel;
using System.Collections.Generic;
using System.Web.Http;

namespace DatanetCMS.WebApi.Controllers
{
    public class SettingController : BaseController
    {
		#region Email Template
	
		[HttpGet]
        public MulitViewResult<EmailTemplateModel> GetEmailTemplates()
        {
            return SettingServiceClient.GetEmailTemplates();
        }

		/// <returns>
		/// 1 = Save Success
		/// 0 = Save Failed
		/// -1 = Subject already exist
		/// </returns>
        [HttpPost]
        public ViewResult<EmailTemplateModel> SaveEmailTemplate(EmailTemplateModel model)
        {
			return SettingServiceClient.SaveEmailTemplate(model);
		}

		/// <returns>
		/// 1 = Delete Success
		/// 0 = Delete Failed
		/// </returns>
		[HttpPost]
        public Operate DeleteEmailTemplate(EmailTemplateModel model)
        {
			return SettingServiceClient.DeleteEmailTemplate(model);
		}

        #endregion
    }
}