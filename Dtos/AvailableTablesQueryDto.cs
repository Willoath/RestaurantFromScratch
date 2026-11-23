using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class AvailableTablesQueryDto : ReservationTimeDto
    {
        [Range(1, int.MaxValue)]
        public int NumberOfGuests { get; set; }
    }
}
