using RestaurantFromScratch.Data;
using RestaurantFromScratch.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Enums;

namespace RestaurantFromScratch.Services
{
    public class ReservationService
    {
        private readonly RestaurantContext _context;

        public ReservationService(RestaurantContext context)
        {
            _context = context;
        }



        public AddReservationResult AddReservation(Reservation reservation)
        {
            var tableExists = _context.Tables.Any(t => t.Id == reservation.TableId);
            if (!tableExists)
            {
                return AddReservationResult.TableNotFound;
            }
            if (!IsTableAvailable(reservation))
            {
                return AddReservationResult.TableAlreadyReserved;
            }
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return AddReservationResult.Success;

        }
        public List<Reservation> GetAllReservations()
        {
            return _context.Reservations.ToList();
        }
        public Reservation? GetReservationById(int id)
        {
            var reservation = _context.Reservations.Include(r => r.Table).FirstOrDefault(r => r.Id == id);

            return reservation;
        }
        private bool IsTableAvailable(Reservation reservation)
        {
            List<Reservation> existingReservations = _context.Reservations.Where(r => r.TableId == reservation.TableId).ToList();
            foreach (var existingReservation in existingReservations)
            {
                if (reservation.ReservationStart < existingReservation.ReservationEnd && reservation.ReservationEnd > existingReservation.ReservationStart)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
