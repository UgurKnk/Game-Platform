using GamePlatform.Business.Dtos;
using GamePlatform.Business.Services;
using GamePlatform.Business.Types;
using GamePlatform.Data.Entities;
using GamePlatform.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlatform.Business.Manager
{
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductManager(IRepository<ProductEntity> productRepository, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }
        public ServiceMessage AddProduct(AddProductDto addProductDto)
        {
            var hasProduct = _productRepository.GetAll(x => x.Name.ToLower() == addProductDto.Name.ToLower()).ToList();
            if (hasProduct.Count > 0)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Dostum kafanı toplasan iyi olur. Bu isimde bir ürün zaten mevcut."
                };
            }
            var productEntity = new ProductEntity()
            {
                Name = addProductDto.Name,
                Description = addProductDto.Description,
                UnitPrice = addProductDto.UnitPrice,
                Storage = addProductDto.Storage,
                Popularity = addProductDto.Popularity,
                ImagePath = addProductDto.ImagePath,
                CategoryId = addProductDto.CategoryId,

            };
            _productRepository.Add(productEntity);
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Yeni ürün eklendi"
            };
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public EditProductDto GetProductById(int id)
        {
            var productEntity = _productRepository.GetById(id);
            var editProductDto = new EditProductDto()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                UnitPrice = productEntity.UnitPrice,
                Storage = productEntity.Storage,
                Popularity = productEntity.Popularity,
                ImagePath = productEntity.ImagePath,
                CategoryId = productEntity.CategoryId,
            };
            return editProductDto;
        }

        public ProductDetailDto GetProductDetail(int id)
        {
            var productEntity = _productRepository.GetById(id);
            var productDto =  new ProductDetailDto()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                UnitPrice = productEntity.UnitPrice,
                Storage = productEntity.Storage,
                Popularity = productEntity.Popularity,
                ImagePath = productEntity.ImagePath,
                CategoryId = productEntity.CategoryId,
            };

            productDto.CategoryName = _categoryService.GetCategoryName(productDto.CategoryId);
            //if (productDto != null)
            //{
            //    return productDto;
            //}
            return productDto;
        }
        
        public List<ProductDto> GetProducts()
        {
            var productEntities = _productRepository.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Popularity);

            var productDtoList = productEntities.Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                Storage = x.Storage,
                Popularity = x.Popularity,
                ImagePath = x.ImagePath,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name
            }).ToList();

            return productDtoList;

        }

        public List<ProductDto> GetProductsByCategoryId(int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                var productEntities = _productRepository.GetAll(x => x.CategoryId == categoryId).OrderBy(x => x.Name);

                var productDto = productEntities.Select(x => new ProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    UnitPrice = x.UnitPrice,
                    Storage = x.Storage,
                    Popularity = x.Popularity,
                    ImagePath = x.ImagePath,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name
                }).ToList();
                return productDto;
            }
            else
            {
                return GetProducts();
            }
        }

        public List<ProductDto> GetProductsOrderByPop()
        {

            var productEntities = _productRepository.GetAll().OrderBy(x => x.Popularity);

            var productDtoList = productEntities.Select(x => new ProductDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                Storage = x.Storage,
                Popularity = x.Popularity,
                ImagePath = x.ImagePath,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name
            }).ToList();

            return productDtoList;
        }

        public void UpdateProduct(EditProductDto editProductDto)
        {
            var productEntity = _productRepository.GetById(editProductDto.Id);
            productEntity.Name = editProductDto.Name;
            productEntity.Description = editProductDto.Description;
            productEntity.UnitPrice = editProductDto.UnitPrice;
            productEntity.Storage = editProductDto.Storage;
            productEntity.Popularity = editProductDto.Popularity;
            productEntity.CategoryId = editProductDto.CategoryId;
            if (editProductDto.ImagePath != null)
            {
                productEntity.ImagePath = editProductDto.ImagePath;
            }
            _productRepository.Update(productEntity);
        }
    }
}
