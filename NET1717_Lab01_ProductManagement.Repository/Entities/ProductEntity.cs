using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1717_Lab01_ProductManagement.Repository.Entities
{
    [Table("Product")]
    public class ProductEntity : ISoftDelete
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public int UnitsInStock { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryEntity CategoryEntity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
