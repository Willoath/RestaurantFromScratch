using RestaurantFromScratch.Enums;
namespace RestaurantFromScratch.Results
{
    public class AddReservationServiceResult
    {
        public AddReservationResult Result { get; set; }
        public int? ReservationId { get; set; }
    }
}
