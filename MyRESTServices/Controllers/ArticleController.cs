using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRESTServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleBLL _articleBLL;

        public ArticleController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL ?? throw new ArgumentNullException(nameof(articleBLL));
        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> Get()
        {
            var articles = await _articleBLL.GetArticleWithCategory();
            return Ok(articles);
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> Get(int id)
        {
            var article = await _articleBLL.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }
            return article;
        }

        // POST api/<ArticleController>
        [HttpPost]
        public async Task<ActionResult<ArticleDTO>> Post([FromBody] ArticleCreateDTO article)
        {
            var insertedArticle = await _articleBLL.Insert(article);
            return CreatedAtAction(nameof(Get), new { id = insertedArticle.ArticleID }, insertedArticle);
        }

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleUpdateDTO article)
        {
            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            var updatedArticle = await _articleBLL.Update(article);
            if (updatedArticle == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _articleBLL.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET api/<ArticleController>/paged
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetWithPaging(int categoryId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var articles = await _articleBLL.GetWithPaging(categoryId, pageNumber, pageSize);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
