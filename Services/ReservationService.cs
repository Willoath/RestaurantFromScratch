using RestaurantFromScratch.Data;
using RestaurantFromScratch.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantFromScratch.Enums;
using RestaurantFromScratch.Dtos;

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
            if (!IsTableAvailable(reservation.TableId, reservation.ReservationStart, reservation.ReservationEnd))
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
        private bool IsTableAvailable(int tableId, DateTime reservationStart, DateTime reservationEnd, int? excludedReservationId = null)
        {
            List<Reservation> existingReservations = _context.Reservations.Where(r => r.TableId == tableId && r.Id != excludedReservationId).ToList();
            foreach (var existingReservation in existingReservations)
            {
                if (reservationStart < existingReservation.ReservationEnd && reservationEnd > existingReservation.ReservationStart)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateReservation(UpdateReservationDto updatedReservation, int id)
        {
            var existingReservation = _context.Reservations.FirstOrDefault(r => r.Id == id);


            if (existingReservation == null || !IsTableAvailable(existingReservation.TableId, updatedReservation.ReservationStart, updatedReservation.ReservationEnd, existingReservation.Id)) {
                return false;
            }
            existingReservation.ReservationStart = updatedReservation.ReservationStart;
            existingReservation.ReservationEnd = updatedReservation.ReservationEnd;
            existingReservation.CustomerName = updatedReservation.CustomerName;
            existingReservation.PhoneNumber = updatedReservation.PhoneNumber;
            existingReservation.NumberOfGuests = updatedReservation.NumberOfGuests;
            existingReservation.Notes = updatedReservation.Notes;
            _context.SaveChanges();
            return true;

        }

    }
}
