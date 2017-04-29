using DatanetCMS.DAO;

namespace DatanetCMS.Service
{
	public class BaseService
	{
		private static DatanetCMSWebEntities _entityClient;
		public static DatanetCMSWebEntities EntityClient => _entityClient ?? (_entityClient = new DatanetCMSWebEntities());
	}
}