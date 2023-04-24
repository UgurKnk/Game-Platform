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
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        public CategoryManager(IRepository<CategoryEntity> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ServiceMessage AddCategory(AddCategoryDto addCategoryDto)
        {
            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == addCategoryDto.Name.ToLower()).ToList();
            if (hasCategory.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Dostum kendine gel! Bu katefori zaten var."
                };
            }
            var categoryEntity = new CategoryEntity()
            {
                Name = addCategoryDto.Name,
                Description = addCategoryDto.Description,

            };
            _categoryRepository.Add(categoryEntity);
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Yeni bir tür. Severim"
            };
        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.Delete(id);
        }

        public List<CategoryDto> GetCategories()
        {
            var categoryEntities = _categoryRepository.GetAll().OrderBy(x => x.Name);
            var categoryDtoList = categoryEntities.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToList();
            return categoryDtoList;
        }

        public EditCategoryDto GetCategory(int id)
        {
            var categoryEntity = _categoryRepository.GetById(id);
            var categoryDto = new EditCategoryDto()
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
            };
            return categoryDto;
        }

        public string GetCategoryName(int id)
        {
            return _categoryRepository.GetById(id).Name;
        }

        public void UpdateCategory(EditCategoryDto editCategoryDto)
        {
            var categoryEntity = _categoryRepository.GetById(editCategoryDto.Id);
            categoryEntity.Name = editCategoryDto.Name;
            categoryEntity.Description = editCategoryDto.Description;
            _categoryRepository.Update(categoryEntity);
        }
    }
}
