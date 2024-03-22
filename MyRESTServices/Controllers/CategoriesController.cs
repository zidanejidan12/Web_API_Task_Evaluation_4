using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryBLL _categoryBLL;

        public CategoriesController(ICategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL ?? throw new ArgumentNullException(nameof(categoryBLL));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var results = await _categoryBLL.GetAll();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var result = await _categoryBLL.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetWithPaging(int pageNumber = 1, int pageSize = 10, string name = null)
        {
            try
            {
                var results = await _categoryBLL.GetWithPaging(pageNumber, pageSize, name);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryCreateDTO categoryCreateDTO)
        {
            if (categoryCreateDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await _categoryBLL.Insert(categoryCreateDTO);
                return CreatedAtAction(nameof(Get), null, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            var result = await _categoryBLL.Update(new CategoryUpdateDTO { CategoryId = id, CategoryName = categoryUpdateDTO.CategoryName });
            if (result == null)
            {
                return NotFound();
            }
            return Ok("Update data success");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] // Restrict access to users with the "admin" role
        public async Task<IActionResult> Delete(int id)
        {
            if (await _categoryBLL.GetById(id) == null)
            {
                return NotFound();
            }

            try
            {
                await _categoryBLL.Delete(id);
                return Ok("Delete data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

