namespace RestaurantFromScratch.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; } = string.Empty;

    }
}
