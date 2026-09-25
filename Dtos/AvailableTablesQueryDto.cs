using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class AvailableTablesQueryDto : IValidatableObject
    {
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        [Range(1, 6)]
        public int NumberOfGuests { get; set; }

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
