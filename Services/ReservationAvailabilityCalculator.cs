using RestaurantFromScratch.Models;

namespace RestaurantFromScratch.Services
{
    public class ReservationAvailabilityCalculator
    {
        public DateTime FindNextAvailableStart(
            DateTime requestedStart,
            TimeSpan duration,
            IEnumerable<Reservation> reservations)
            {
            var candidateStart = requestedStart;

            foreach (var existingReservation in reservations
                .OrderBy(r => r.ReservationStart))
            {
                var candidateEnd = candidateStart + duration;

                if (candidateEnd <= existingReservation.ReservationStart)
                {
                    break;
                }

                if (candidateStart < existingReservation.ReservationEnd &&
                    candidateEnd > existingReservation.ReservationStart)
                {
                    candidateStart = existingReservation.ReservationEnd;
                }
            }

            return candidateStart;
        }
    }
}