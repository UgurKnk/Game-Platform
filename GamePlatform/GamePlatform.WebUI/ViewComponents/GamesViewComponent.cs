using GamePlatform.Business.Dtos;
using GamePlatform.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.WebUI.Models
{
    public class GamesViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public GamesViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public IViewComponentResult Invoke()
        {
            var productDtos = _productService.GetProductsOrderByPop();

            var viewModel = productDtos.Select(x => new ProductViewModel
            {

                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                Storage = x.Storage,
                Popularity = x.Popularity,
                ImagePath = x.ImagePath,
                CategoryName= x.CategoryName,

            }).ToList();

            return View(viewModel);

        }
    }
}
