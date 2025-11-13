using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Dto.CategoryDto
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must be less than 500 characters")]
        public string? Description { get; set; }
    }
}
