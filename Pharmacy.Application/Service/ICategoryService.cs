using Pharmacy.Dto.CategoryDto;
using Pharmacy.Dto.Shared;
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
        public Task<Result<PaginatedResponse<CategoryReadDTO>>> GetAllCategories(int PageNum =1 , int Pagesize =10);  

        public Task<Result> Create(CategoryCreateDTO createCategoryDto);  //<CategoryReadDTO>  شيلناه لان اصلا هعمل ريدايركت 
        public Task Update(CategoryCreateDTO createCategoryDto);
        public Task Delete(int CategoryId);


    }
}
