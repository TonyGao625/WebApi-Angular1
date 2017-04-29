using DatanetCMS.DAO;
using DatanetCMS.Model;
using DatanetCMS.Model.SettingModel;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DatanetCMS.Service
{
	public class SettingService : BaseService
	{
		public MulitViewResult<EmailTemplateModel> GetEmailTemplates()
		{
            var result = new MulitViewResult<EmailTemplateModel>();

            result.Datas = EntityClient.EmailTemplates.Select(t => new EmailTemplateModel
            {
                Id = t.Id,
                Subject = t.Subject,
                Body = t.Body
            }).ToList();

            return result;
		}

		/// <returns>
		/// 1 = Save Success
		/// 0 = Save Failed
		/// -1 = Subject already exist
		/// </returns>
		public ViewResult<EmailTemplateModel> SaveEmailTemplate(EmailTemplateModel model)
		{
			/* Update */
			var existDao = EntityClient.EmailTemplates.Find(model.Id);
			if (existDao != null)
			{
				existDao.Subject = model.Subject;
				existDao.Body = model.Body;
				EntityClient.EmailTemplates.AddOrUpdate(existDao);
				EntityClient.SaveChanges();
				return new ViewResult<EmailTemplateModel>
				{
					Status = EntityClient.SaveChanges() > 0 ? 1 : 0,
					Data = model
				};
			}

			///* Exist Checking */
			//if (EntityClient.EmailTemplates.Any(t => t.Subject == model.Subject))
			//{
			//	return new ViewResult<EmailTemplateModel>
			//	{
			//		Status = -1,
			//		Message = "Subject already exist"
			//	};
			//}

			/* Save New */
			var newDao = new EmailTemplate
			{
				Subject = model.Subject,
				Body = model.Body
			};
			EntityClient.EmailTemplates.Add(newDao);
			EntityClient.SaveChanges();
			model.Id = newDao.Id;
			return new ViewResult<EmailTemplateModel>
			{
				Status = EntityClient.SaveChanges() > 0 ? 1 : 0,
				Data = model
			};
		}

		/// <returns>
		/// 1 = Delete Success
		/// 0 = Delete Failed
		/// </returns>
		public Operate DeleteEmailTemplate(EmailTemplateModel model)
		{
			var existDao = EntityClient.EmailTemplates.Find(model.Id);
			if (existDao == null) return new Operate {Status = 0};

			EntityClient.EmailTemplates.Remove(existDao);
			return new Operate { Status = EntityClient.SaveChanges() > 0 ? 1 : 0 };
		}
	}
}
