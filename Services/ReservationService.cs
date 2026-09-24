using RestaurantFromScratch.Data;
using RestaurantFromScratch.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantFromScratch.Enums;
using RestaurantFromScratch.Dtos;
using RestaurantFromScratch.Results;

namespace RestaurantFromScratch.Services
{
    public class ReservationService
    {
        private readonly RestaurantContext _context;

        public ReservationService(RestaurantContext context)
        {
            _context = context;
        }



        public AddReservationServiceResult AddReservation(CreateReservationDto reservation)
        {
            var table = _context.Tables.FirstOrDefault(t => t.Id == reservation.TableId);
            if (table == null)
            {
                return new AddReservationServiceResult { Result = AddReservationResult.TableNotFound };
            }
            if (!IsThereEnoughSeats(table.Seats, reservation.NumberOfGuests))
            {
                return new AddReservationServiceResult { Result = AddReservationResult.NotEnoughSeats };
            }
            if (!IsTableAvailable(reservation.TableId, reservation.ReservationStart, reservation.ReservationEnd))
            {
                return new AddReservationServiceResult { Result = AddReservationResult.TableAlreadyReserved };
            }
            var newReservation = new Reservation
            {
                CustomerName = reservation.CustomerName,
                PhoneNumber = reservation.PhoneNumber,
                ReservationStart = reservation.ReservationStart,
                ReservationEnd = reservation.ReservationEnd,
                NumberOfGuests = reservation.NumberOfGuests,
                Notes = reservation.Notes,
                TableId = reservation.TableId
            };
            _context.Reservations.Add(newReservation);
            _context.SaveChanges();
            return new AddReservationServiceResult { Result = AddReservationResult.Success, ReservationId = newReservation.Id };

        }
        public List<ReservationDto> GetAllReservations()
        {
            return _context.Reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CustomerName = r.CustomerName,
                PhoneNumber = r.PhoneNumber,
                ReservationStart = r.ReservationStart,
                ReservationEnd = r.ReservationEnd,
                NumberOfGuests = r.NumberOfGuests,
                Notes = r.Notes,
                TableId = r.TableId,
                TableNumber = r.Table.TableNumber,
                TableSeats = r.Table.Seats
            }).ToList();
        }
        public ReservationDto? GetReservationById(int id)
        {
            var reservation = _context.Reservations.Include(r => r.Table).FirstOrDefault(r => r.Id == id);
            if (reservation == null) { return null; }

            return new ReservationDto
            {
                Id = reservation.Id,
                CustomerName = reservation.CustomerName,
                PhoneNumber = reservation.PhoneNumber,
                ReservationStart = reservation.ReservationStart,
                ReservationEnd = reservation.ReservationEnd,
                NumberOfGuests = reservation.NumberOfGuests,
                Notes = reservation.Notes,
                TableId = reservation.TableId,
                TableNumber = reservation.Table.TableNumber,
                TableSeats = reservation.Table.Seats
            };
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
        private bool IsThereEnoughSeats(int seats, int numberOfGuests)
        {
            return seats >= numberOfGuests;
        }
        public UpdateReservationResult UpdateReservation(UpdateReservationDto updatedReservation, int id)
        {
            var existingReservation = _context.Reservations.Include(r => r.Table).FirstOrDefault(r => r.Id == id);


            if (existingReservation == null)
            {
                return UpdateReservationResult.ReservationNotFound;
            }
            if (!IsThereEnoughSeats(existingReservation.Table.Seats,updatedReservation.NumberOfGuests))
            {
                return UpdateReservationResult.NotEnoughSeats;
            }
            if (!IsTableAvailable(existingReservation.TableId, updatedReservation.ReservationStart, updatedReservation.ReservationEnd, existingReservation.Id))
            {
                return UpdateReservationResult.TableAlreadyReserved;
            }
            existingReservation.ReservationStart = updatedReservation.ReservationStart;
            existingReservation.ReservationEnd = updatedReservation.ReservationEnd;
            existingReservation.CustomerName = updatedReservation.CustomerName;
            existingReservation.PhoneNumber = updatedReservation.PhoneNumber;
            existingReservation.NumberOfGuests = updatedReservation.NumberOfGuests;
            existingReservation.Notes = updatedReservation.Notes;
            _context.SaveChanges();
            return UpdateReservationResult.Success;

        }
        public bool DeleteReservation(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return false;
            }
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return true;
        }

    }
}
