using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ReservationStart { get; set; }
        [Range(1,6)]
        public int NumberOfGuests { get; set; }
        public DateTime ReservationEnd { get; set; }
        public string? Notes { get; set; }
        [Range(1,int.MaxValue)]
        public int TableId { get; set; }
        public Table Table { get; set; } = null!;


    }
}
