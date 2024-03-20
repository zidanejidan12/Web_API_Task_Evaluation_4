using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRESTServices.BLL
{
    public class CategoryBLL : ICategoryBLL
    {
        private readonly ICategoryData _categoryData;
        private readonly IMapper _mapper;

        public CategoryBLL(ICategoryData categoryData, IMapper mapper)
        {
            _categoryData = categoryData ?? throw new ArgumentNullException(nameof(categoryData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Delete(int id)
        {
            return await _categoryData.Delete(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await _categoryData.GetAll();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var category = await _categoryData.GetById(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetByName(string name)
        {
            var categories = await _categoryData.GetByName(name);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<int> GetCountCategories(string name)
        {
            return await _categoryData.GetCountCategories(name);
        }

        public async Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            var categories = await _categoryData.GetWithPaging(pageNumber, pageSize, name);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> Insert(CategoryCreateDTO entity)
        {
            var category = _mapper.Map<Category>(entity);
            var insertedCategory = await _categoryData.Insert(category);
            return _mapper.Map<CategoryDTO>(insertedCategory);
        }

        public async Task<CategoryDTO> Update(CategoryUpdateDTO entity)
        {
            var existingCategory = await _categoryData.GetById(entity.CategoryId);
            if (existingCategory == null)
                return null;

            // Check if the updated name is the same as the existing name
            if (existingCategory.CategoryName != entity.CategoryName)
            {
                // Check if the new name conflicts with any existing category
                var existingCategoryWithSameName = await _categoryData.GetByName(entity.CategoryName);
                if (existingCategoryWithSameName.Any())
                {
                    // Handle case where the new name conflicts with an existing category
                    return null; // Or throw an exception indicating name conflict
                }
            }

            // Update the name
            existingCategory.CategoryName = entity.CategoryName;

            // Perform the update in the data layer
            var updatedCategory = await _categoryData.Update(entity.CategoryId, existingCategory); // Pass both id and updated entity

            return _mapper.Map<CategoryDTO>(updatedCategory);
        }
    }
}
