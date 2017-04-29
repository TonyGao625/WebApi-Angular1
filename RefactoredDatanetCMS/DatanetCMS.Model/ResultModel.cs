using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DatanetCMS.Model
{
    public class ResultModel
    {

    }

    public class Operate
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class ViewResult<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class MulitViewResult<T>
    {
        public MulitViewResult()
        {
            Datas = new List<T>();
        }
        public int Status { get; set; }
        public string Message { get; set; }
        public List<T> Datas { get; set; }
        public int AllCount { get; set; }
    }

    public class ProductsResult
    {
        public int Status { get; set; }
        public string Error { get; set; }
        public List<ProductResult> Sources { get; set; }

        public ProductsResult()
        {
            Sources = new List<ProductResult>();
        }
    }

    public class ProductResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Template { get; set; }
        public string TemplateName { get; set; }
        public JObject Source { get; set; }
    }
}
