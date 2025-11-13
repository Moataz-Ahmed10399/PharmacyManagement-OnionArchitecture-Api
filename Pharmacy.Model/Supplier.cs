using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    public class Supplier : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }

        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
