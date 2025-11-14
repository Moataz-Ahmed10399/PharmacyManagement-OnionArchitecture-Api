using Mapster;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interface;
using Pharmacy.Dto.CategoryDto;
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

        public Task Create(CategoryCreateDTO createCategoryDto)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryReadDTO>> GetAllCategories()
        {
            //var allcat = (await _CategoryRepo.GetAll()).ToListAsync();

            //List<CategoryReadDTO> res = allcat.Adapt<List<CategoryReadDTO>>();
            //return res;
            var allcat = (await _CategoryRepo.GetAll()).ToList(); // مش ToListAsync
            var res = allcat.Adapt<List<CategoryReadDTO>>();
            return res;
        }

        public Task Update(CategoryCreateDTO createCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
