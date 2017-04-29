using DatanetCMS.DAO;
using DatanetCMS.Model.CategoryModel;
using DatanetCMS.Model.UomModel;

namespace DatanetCMS.Model.ProductModel
{
    public static class ProductModelConvert
    {
        public static ProductModel ToProductModel(this Product product)
        {
            var model = new ProductModel()
            {
                Id = product.Id,
                Code = product.Code,
                ShortDesc = product.ShortDesc,
                LongDesc = product.LongDesc,
                Vendor = product.Vendor,
                UomId = product.UomId,
                Image = product.Image,
                CategoryId = product.CategoryId
            };
            if (model.UomId != 0 && product.Uom != null)
            {
                model.Uom = product.Uom.ToUomModel();
            }
            if (model.CategoryId!=0 && product.Category !=null)
            {
                model.Category = product.Category.ToCategoryModel();
            }
            return model;
        }


        public static ProductModel ToProductModelWithoutImage(this Product product)
        {
            var model = new ProductModel()
            {
                Id = product.Id,
                Code = product.Code,
                ShortDesc = product.ShortDesc,
                LongDesc = product.LongDesc,
                Vendor = product.Vendor,
                UomId = product.UomId,
                CategoryId = product.CategoryId
            };
            if (model.UomId != 0 && product.Uom != null)
            {
                model.Uom = product.Uom.ToUomModel();
            }
            if (model.CategoryId != 0 && product.Category != null)
            {
                model.Category = product.Category.ToCategoryModel();
            }
            return model;
        }

        public static ProductModel ToProductModelWithImage(this Product product)
        {
            var model = new ProductModel()
            {
                Id = product.Id,
                Code = product.Code,
                ShortDesc = product.ShortDesc,
                LongDesc = product.LongDesc,
                Vendor = product.Vendor,
                UomId = product.UomId,
                CategoryId = product.CategoryId,
                Image = product.Image
            };
            if (model.UomId != 0 && product.Uom != null)
            {
                model.Uom = product.Uom.ToUomModel();
            }
            if (model.CategoryId != 0 && product.Category != null)
            {
                model.Category = product.Category.ToCategoryModel();
            }
            return model;
        }

        public static Product ToProductModel(this ProductModel product)
        {
            var model = new Product()
            {
                Code = product.Code,
                ShortDesc = product.ShortDesc,
                LongDesc = product.LongDesc,
                Vendor = product.Vendor,
                UomId = product.UomId,
                Image = product.Image,
                CategoryId = product.CategoryId
            };
            return model;
        }

        public static ProductModel ToQuoteProductModel(this Product product)
        {
            var model = new ProductModel()
            {
                Id = product.Id,
                Code = product.Code,
                ShortDesc = product.ShortDesc,
                LongDesc = product.LongDesc,
                Vendor = product.Vendor,
                UomId = product.UomId,
                Image = product.Image,
                CategoryId = product.CategoryId
            };
            return model;
        }
    }
}
