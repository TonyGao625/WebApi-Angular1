using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Model
{
    public class FilterModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Mode { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string OrderCode { get; set; }
        public int? page { get; set; }
        public  int? pageSize { get; set; }
        public int ManagerId { get; set; }
    }
}
