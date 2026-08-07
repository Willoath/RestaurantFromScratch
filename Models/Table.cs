using System.ComponentModel.DataAnnotations;
namespace RestaurantFromScratch.Models
{
    public class Table
    {
        public int Id { get; set; }
        [Range(1,int.MaxValue)]
        public int TableNumber { get; set; }
        [Range(1,6)]
        public int Seats { get; set; }
        public bool IsActive { get; set; }
    }
}
