using System.ComponentModel.DataAnnotations;

namespace Product_Inventory_System.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }            
        public string Name { get; set; }              
        public string Category { get; set; }       
        public DateTime CreatedAt { get; set; }       
        public DateTime? UpdatedAt { get; set; }      
    }
}
