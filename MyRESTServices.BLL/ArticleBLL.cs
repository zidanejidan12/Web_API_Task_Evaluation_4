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
    public class ArticleBLL : IArticleBLL
    {
        private readonly IArticleData _articleData;
        private readonly IMapper _mapper;

        public ArticleBLL(IArticleData articleData, IMapper mapper)
        {
            _articleData = articleData ?? throw new ArgumentNullException(nameof(articleData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Delete(int id)
        {
            return await _articleData.Delete(id);
        }

        public async Task<ArticleDTO> GetArticleById(int id)
        {
            var article = await _articleData.GetById(id);
            return _mapper.Map<ArticleDTO>(article);
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleByCategory(int categoryId)
        {
            var articles = await _articleData.GetArticleByCategory(categoryId);
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleWithCategory()
        {
            var articles = await _articleData.GetArticleWithCategory();
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<int> GetCountArticles()
        {
            return await _articleData.GetCountArticles();
        }

        public async Task<IEnumerable<ArticleDTO>> GetWithPaging(int categoryId, int pageNumber, int pageSize)
        {
            var articles = await _articleData.GetWithPaging(categoryId, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<int> InsertWithIdentity(ArticleCreateDTO article)
        {
            var articleEntity = _mapper.Map<Article>(article);
            return await _articleData.InsertWithIdentity(articleEntity);
        }

        public async Task<ArticleDTO> Insert(ArticleCreateDTO article)
        {
            var articleEntity = _mapper.Map<Article>(article);
            var insertedArticle = await _articleData.Insert(articleEntity);
            return _mapper.Map<ArticleDTO>(insertedArticle);
        }

        public async Task<ArticleDTO> Update(ArticleUpdateDTO article)
        {
            var existingArticle = await _articleData.GetById(article.ArticleID);
            if (existingArticle == null)
                return null;

            existingArticle.Title = article.Title;
            existingArticle.Details = article.Details;
            existingArticle.CategoryId = article.CategoryID;
            existingArticle.IsApproved = article.IsApproved;
            existingArticle.Pic = article.Pic;

            var updatedArticle = await _articleData.Update(article.ArticleID, existingArticle);

            return _mapper.Map<ArticleDTO>(updatedArticle);
        }
    }
}
