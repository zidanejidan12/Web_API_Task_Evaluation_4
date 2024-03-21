using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRESTServices.Data
{
    public class ArticleData : IArticleData
    {
        private readonly AppDbContext _context;

        public ArticleData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _context.Articles.ToListAsync();
        }


        public async Task<Article> GetById(int id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.ArticleId == id);
        }

        public async Task<int> GetCountArticles()
        {
            return await _context.Articles.CountAsync();
        }

        public async Task<IEnumerable<Article>> GetWithPaging(int categoryId, int pageNumber, int pageSize)
        {
            return await _context.Articles
                .Where(a => a.CategoryId == categoryId)
                .Include(a => a.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticleWithCategory()
        {
            return await _context.Articles.Include(a => a.Category).ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticleByCategory(int categoryId)
        {
            return await _context.Articles.Where(a => a.CategoryId == categoryId).Include(a => a.Category).ToListAsync();
        }

        public async Task<int> InsertWithIdentity(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article.ArticleId;
        }

        public async Task<Task> InsertArticleWithCategory(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<Article> Insert(Article entity)
        {
            _context.Articles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Article> Update(int id, Article entity)
        {
            if (!_context.Articles.Any(a => a.ArticleId == id))
                return null;

            entity.ArticleId = id;
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                    return null;
                else
                    throw;
            }

            return entity;
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(a => a.ArticleId == id);
        }
    }
}
