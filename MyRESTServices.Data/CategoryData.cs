using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRESTServices.Data
{
    public class CategoryData : ICategoryData
    {
        private readonly AppDbContext _context;
        public CategoryData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetByName(string name)
        {
            return await _context.Categories.Where(c => c.CategoryName.Contains(name)).ToListAsync();
        }

        public async Task<int> GetCountCategories(string name)
        {
            return await _context.Categories.CountAsync(c => c.CategoryName.Contains(name));
        }

        public async Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(name))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Category> Insert(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<int> InsertWithIdentity(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> Update(int id, Category entity)
        {
            if (!_context.Categories.Any(c => c.CategoryId == id))
                return null;

            entity.CategoryId = id;
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                    return null;
                else
                    throw;
            }

            return entity;
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        }
    }
}
