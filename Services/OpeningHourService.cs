using RestaurantFromScratch.Data;

namespace RestaurantFromScratch.Services
{
    public class OpeningHoursService
    {
        private readonly RestaurantContext _context;

        public OpeningHoursService(RestaurantContext context)
        {
            _context = context;
        }

        public bool IsRestaurantOpen(DateTime reservationStart,DateTime reservationEnd)
        {
            if (reservationStart.Date != reservationEnd.Date || reservationStart >= reservationEnd)
            {
                return false;
            }

            var dayOfWeek = reservationStart.DayOfWeek;
            var openingHour = _context.OpeningHours.FirstOrDefault(o => o.DayOfWeek == dayOfWeek);

            if (openingHour == null || openingHour.IsClosed)
            {
                return false; 
            }
            if (openingHour.OpenTime == null || openingHour.CloseTime == null)
            {
                return false;
            }

            var startTime = TimeOnly.FromDateTime(reservationStart);
            var endTime = TimeOnly.FromDateTime(reservationEnd);

            var openTime = openingHour.OpenTime.Value;
            var closeTime = openingHour.CloseTime.Value;

            return startTime >= openTime && endTime <= closeTime;
        }
    }
}
