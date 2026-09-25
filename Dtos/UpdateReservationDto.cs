using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class UpdateReservationDto : ReservationTimeDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Range(1, 6)]
        public int NumberOfGuests { get; set; }
        public string? Notes { get; set; }

    }
}
