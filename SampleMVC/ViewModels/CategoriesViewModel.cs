using System.Collections.Generic;
using MyWebFormApp.BLL.DTOs;

namespace SampleMVC.ViewModels
{
    public class CategoriesViewModel
    {
        public IEnumerable<CategoryDTO>? Categories { get; set; }
        public CategoryCreateDTO? CategoryCreateDTO { get; set; }
        public CategoryUpdateDTO? CategoryUpdateDTO { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
    }
}
