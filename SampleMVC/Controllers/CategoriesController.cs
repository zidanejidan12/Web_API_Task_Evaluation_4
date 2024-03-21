using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using SampleMVC.Services;
using SampleMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices ?? throw new ArgumentNullException(nameof(categoryServices));
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all categories
                var categories = await _categoryServices.GetAll();

                // Populate other properties of the view model
                var viewModel = new CategoriesViewModel
                {
                    Categories = categories
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                // Handle the exception, either by showing an error view or redirecting to an error page
                return View("Error");
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryServices.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriesViewModel categoriesViewModel)
        {
            if (ModelState.IsValid)
            {
                await _categoryServices.Insert(categoriesViewModel.CategoryCreateDTO);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryServices.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryUpdateDTO category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _categoryServices.Update(id, category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryServices.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetWithPaging(int pageNumber = 1, int pageSize = 10, string name = null)
        {
            var categories = await _categoryServices.GetWithPaging(pageNumber, pageSize, name);
            return View(categories);
        }
    }
}
