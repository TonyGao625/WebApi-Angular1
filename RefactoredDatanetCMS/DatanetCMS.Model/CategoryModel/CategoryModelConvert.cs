using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatanetCMS.DAO;

namespace DatanetCMS.Model.CategoryModel
{
    public static class CategoryModelConvert
    {
        public static CategoryModel ToCategoryModel(this Category category)
        {
            var model = new CategoryModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
            return model;
        }
    }
}
