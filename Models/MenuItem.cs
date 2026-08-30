using RestaurantFromScratch.Enums;
using System.ComponentModel.DataAnnotations;
namespace RestaurantFromScratch.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public Category Category { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public string? Description { get; set; } = string.Empty;

    }
}
