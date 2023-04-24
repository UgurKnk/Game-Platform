using GamePlatform.Business.Dtos;
using GamePlatform.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlatform.Business.Services
{
    public interface IProductService
    {
        ServiceMessage AddProduct(AddProductDto addProductDto);

        List<ProductDto> GetProducts();

        EditProductDto GetProductById(int id);

        void UpdateProduct(EditProductDto editProductDto);

        void DeleteProduct(int id);

        List<ProductDto> GetProductsByCategoryId(int? categoryId = null);

        ProductDetailDto GetProductDetail(int id);

        List<ProductDto> GetProductsOrderByPop();
    }
}
