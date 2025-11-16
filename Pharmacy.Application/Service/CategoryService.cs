using Mapster;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interface;
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
    public class CategoryService : IcategoryService
    {
        private readonly IGenericRepo<Category> _CategoryRepo;
        public CategoryService(IGenericRepo<Category> catrepo)
        {
            _CategoryRepo = catrepo;
        }

        public async Task<Result> Create(CategoryCreateDTO createCategoryDto)
        {
            var allcat = await _CategoryRepo.GetAll().AnyAsync(c => c.Name == createCategoryDto.Name);

            if (allcat)
                return Result.Failure("Name of category is Already Taken Controller"); 
                       

            var category = createCategoryDto.Adapt<Category>();

            await _CategoryRepo.Create(category);
            int rescount =   await _CategoryRepo.SaveChangesAsync();

            if(rescount <= 0)
                return Result.Failure("Please Try Again");

            return Result.Success();
        }

        public Task Delete(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<PaginatedResponse<CategoryReadDTO>>> GetAllCategories(int PageNum = 1, int Pagesize =10)
        {
            //var allcat = (await _CategoryRepo.GetAll()).ToListAsync();

            //List<CategoryReadDTO> res = allcat.Adapt<List<CategoryReadDTO>>();
            //return res;



            var allcat = await _CategoryRepo.GetAll(c => c.Medicines).Skip(Pagesize* (PageNum-1)).Take(Pagesize).ToListAsync();

            var count = await _CategoryRepo.GetAll().CountAsync();
         
            List<CategoryReadDTO> res = allcat.Adapt<List<CategoryReadDTO>>();

            PaginatedResponse<CategoryReadDTO> pageresponse = new(res, count , PageNum, Pagesize);

            return Result<PaginatedResponse<CategoryReadDTO>>.Success(pageresponse) ;
        }

        public Task Update(CategoryCreateDTO createCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
