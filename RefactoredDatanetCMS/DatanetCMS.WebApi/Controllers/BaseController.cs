using System.Web.Http;
using DatanetCMS.Service;

namespace DatanetCMS.WebApi.Controllers
{
    public class BaseController : ApiController
    {
	    private static SettingService _settingServiceClient;
		public static SettingService SettingServiceClient => _settingServiceClient ?? (_settingServiceClient = new SettingService());
	}
}
