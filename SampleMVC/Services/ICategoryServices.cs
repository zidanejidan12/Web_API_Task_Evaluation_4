using MyWebFormApp.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleMVC.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetById(int id);
        Task<CategoryDTO> Insert(CategoryCreateDTO category);
        Task<CategoryDTO> Update(int id, CategoryUpdateDTO category);
        Task Delete(int id);
        Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber = 1, int pageSize = 10, string name = null);
    }
}
