using RestaurantFromScratch.Data;
using RestaurantFromScratch.Models; 

namespace RestaurantFromScratch.Services
{
    public class ReservationService
    {
        private readonly RestaurantContext _context;
        
        public ReservationService(RestaurantContext context)
        {
            _context = context;
        }

        public bool AddReservation(Reservation reservation)
        {
            var table = _context.Tables.FirstOrDefault(t => t.Id == reservation.TableId);
            if (table == null)
            {
                return false;
            }
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return true;

        }
        public List<Reservation> GetAllReservations()
        {
            return _context.Reservations.ToList();
        }
        public Reservation? GetReservationById(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            return reservation;
        }
    }
}
