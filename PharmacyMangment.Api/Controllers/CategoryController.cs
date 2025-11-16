using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.Service;
using Pharmacy.Dto.CategoryDto;
using System.Threading.Tasks;

namespace Pharmacy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IcategoryService _categoryService;

        public CategoryController(IcategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDTO>>> GetAll()
        {
            var cats = await _categoryService.GetAllCategories(1,2);

            return Ok(cats);
        }



        [HttpPost]
        public async Task<ActionResult> createcategory([FromBody] CategoryCreateDTO catcreatdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _categoryService.Create(catcreatdto);


            if (!created.IsSuccess)
                return BadRequest(new { message = "Category name is already taken. Api" });

            return Ok(new { message = "Category created successfully." });
        }

    }
}
