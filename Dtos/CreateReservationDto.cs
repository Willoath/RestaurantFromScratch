using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class CreateReservationDto : IValidatableObject
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
        [Range(1, int.MaxValue)]
        public int TableId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReservationStart >= ReservationEnd)
            {
                yield return new ValidationResult("Reservation start time must be before end time.", new[] { nameof(ReservationStart), nameof(ReservationEnd) });
            }
            if (ReservationStart <= DateTime.Now)
            {
                yield return new ValidationResult("Reservation start time must be in the future.", new[] { nameof(ReservationStart) });
            }
        }
    }
}
