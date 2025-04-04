using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("ProductsCategories")]
public class ProductCategory
{
    [Key]
    public Guid ProductCategoryId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public Category? Category { get; set; }
    public Product? Product { get; set; }
}