using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class CreateReservationDto : ReservationTimeDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Phone number must contain exactly 9 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Range(1, 6)]
        public int NumberOfGuests { get; set; }
        public string? Notes { get; set; }
        [Range(1, int.MaxValue)]
        public int TableId { get; set; }

    }
}
