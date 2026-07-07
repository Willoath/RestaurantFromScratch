using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantFromScratch.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ReservatioDate { get; set; }
        public int NumberOfGuests { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Notes { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; } = null!;




    }
}
