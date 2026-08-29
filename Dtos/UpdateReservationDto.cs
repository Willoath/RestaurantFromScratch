using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class UpdateReservationDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ReservationStart { get; set; }
        [Range(1, 6)]
        public int NumberOfGuests { get; set; }
        public DateTime ReservationEnd { get; set; }
        public string? Notes { get; set; }

    }
}
