using System.ComponentModel.DataAnnotations;

namespace RestaurantFromScratch.Dtos
{
    public class AvailableTablesQueryDto : ReservationTimeDto
    {
        [Range(1, 6)]
        public int NumberOfGuests { get; set; }
    }
}
