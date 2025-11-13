using Pharmacy.Dto.CategoryDto;
using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Service
{
    public interface IcategoryService
    {
        public Task<List<CategoryReadDTO>> GetAllCategories();  

        public Task Create(CategoryCreateDTO createCategoryDto);  //<CategoryReadDTO>  شيلناه لان اصلا هعمل ريدايركت 
        public Task Update(CategoryCreateDTO createCategoryDto);
        public Task Delete(int CategoryId);


    }
}
