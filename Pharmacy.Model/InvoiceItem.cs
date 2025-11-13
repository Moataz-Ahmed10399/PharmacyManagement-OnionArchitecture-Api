using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    public class InvoiceItem : BaseEntity
    {
        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; } = null!;

        [ForeignKey("MedicineId")]
        public Medicine Medicine { get; set; } = null!;
    }
}
