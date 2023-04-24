using GamePlatform.Business.Dtos;
using GamePlatform.Business.Services;
using GamePlatform.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;
        public ProductController(ICategoryService categoryService, IProductService productService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _productService = productService;
            _environment = environment;
        }

        public IActionResult List()
        {
            var productDtos = _productService.GetProducts();
            var viewModel = productDtos.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.CategoryName,
                UnitPrice = x.UnitPrice,
                Storage = x.Storage,
                Popularity = x.Popularity,
                ImagePath = x.ImagePath,
            }).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult New()
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", new ProductFormViewModel());
        }
        [HttpPost]
        public IActionResult Save(ProductFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Form", formData);
            }

            var newFileName = "";

            if (formData.File != null)
            {
                var allowedFileContentTypes = new string[] { "image/jpeg", "image/jpg", "image/png", "image/jfif" };

                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif" };

                var fileContentType = formData.File.ContentType;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);

                var fileExtension = Path.GetExtension(formData.File.FileName);

                if (!allowedFileContentTypes.Contains(fileContentType) || !allowedFileExtensions.Contains(fileExtension))
                {


                    ViewBag.FileError = "Dosya formatı veya içeriği hatalı.";

                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);
                }

                newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;


                var folderPath = Path.Combine("images", "products");


                var wwwRootFolderPath = Path.Combine(_environment.WebRootPath, folderPath);


                var wwwRootFilePath = Path.Combine(wwwRootFolderPath, newFileName);



                Directory.CreateDirectory(wwwRootFolderPath);

                using (var fileStream = new FileStream(wwwRootFilePath, FileMode.Create))
                {
                    formData.File.CopyTo(fileStream);
                }
            }

            if (formData.Id == 0)
            {

                var addProductDto = new AddProductDto()
                {
                    Name = formData.Name.Trim(),
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    Storage = formData.Storage,
                    Popularity = formData.Popularity,
                    CategoryId = formData.CategoryId,
                    ImagePath = newFileName
                };

                var response = _productService.AddProduct(addProductDto);

                if (response.IsSucceed)
                {
                    return RedirectToAction("List");

                }
                else
                {
                    ViewBag.ErrorMessage = response.Message;
                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);

                }

            }
            else
            {

                var editProductDto = new EditProductDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    Storage = formData.Storage,
                    Popularity = formData.Popularity,
                    UnitPrice = formData.UnitPrice,
                    CategoryId = formData.CategoryId
                };

                if (formData.File != null)
                    editProductDto.ImagePath = newFileName;


                _productService.UpdateProduct(editProductDto);



            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editProductDto = _productService.GetProductById(id);

            var viewModel = new ProductFormViewModel()
            {
                Id = editProductDto.Id,
                Name = editProductDto.Name,
                Description = editProductDto.Description,
                Storage = editProductDto.Storage,
                UnitPrice = editProductDto.UnitPrice,
                CategoryId = editProductDto.CategoryId,
                Popularity = editProductDto.Popularity
            };

            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", viewModel);
        }

        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("List");
        }
    }
}

