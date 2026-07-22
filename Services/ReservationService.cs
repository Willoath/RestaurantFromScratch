using RestaurantFromScratch.Data;

namespace RestaurantFromScratch.Services
{
    public class ReservationService
    {
        private readonly RestaurantContext _context;
        
        public ReservationService(RestaurantContext context)
        {
            _context = context;
        }
    }
}
