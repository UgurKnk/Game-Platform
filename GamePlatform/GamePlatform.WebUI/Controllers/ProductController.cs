using GamePlatform.Business.Services;
using GamePlatform.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        
        public IActionResult Detail(int id)
        {
            var productDetailDto = _productService.GetProductDetail(id);

            var viewModel = new ProductDetailViewModel
            {
                Id = productDetailDto.Id,
                Name = productDetailDto.Name,
                Description = productDetailDto.Description,
                UnitPrice = productDetailDto.UnitPrice,
                ImagePath = productDetailDto.ImagePath,
                CategoryId = productDetailDto.CategoryId,
                CategoryName = productDetailDto.CategoryName,
                Storage = productDetailDto.Storage,
                Popularity = productDetailDto.Popularity
            };
            return View(viewModel);
        }
    }
}
