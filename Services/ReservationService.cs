using Microsoft.EntityFrameworkCore;
using RestaurantFromScratch.Data;
using RestaurantFromScratch.Dtos;
using RestaurantFromScratch.Enums;
using RestaurantFromScratch.Models;
using RestaurantFromScratch.Results;


namespace RestaurantFromScratch.Services
{
    public class ReservationService
    {
        private readonly RestaurantContext _context;
        private readonly OpeningHoursService _openingHoursService;

        private readonly ReservationAvailabilityCalculator _availabilityCalculator;

        public ReservationService(RestaurantContext context, OpeningHoursService openingHoursService, ReservationAvailabilityCalculator availabilityCalculator)
        {
            _context = context;
            _openingHoursService = openingHoursService;
            _availabilityCalculator = availabilityCalculator;
        }
        



        public AddReservationServiceResult AddReservation(CreateReservationDto reservation)
        {
            var table = _context.Tables.FirstOrDefault(t => t.Id == reservation.TableId && t.IsActive == true);
            if (table == null)
            {
                return new AddReservationServiceResult { Result = AddReservationResult.TableNotFound };
            }
            if (!IsTableAvailable(reservation.TableId, reservation.ReservationStart, reservation.ReservationEnd))
            {
                return new AddReservationServiceResult { Result = AddReservationResult.TableAlreadyReserved };
            }
            if (!_openingHoursService.IsRestaurantOpen(reservation.ReservationStart, reservation.ReservationEnd))
            {
                return new AddReservationServiceResult { Result = AddReservationResult.RestaurantClosed };
            }
            if (!IsThereEnoughSeats(table.Seats, reservation.NumberOfGuests))
            {
                return new AddReservationServiceResult { Result = AddReservationResult.NotEnoughSeats };
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
        public NextAvailableReservationDto? GetNextAvailableReservation(AvailableTablesQueryDto query)
        {
            var duration = query.ReservationEnd - query.ReservationStart;
            var suitableTables = _context.Tables
                .Where(t => t.Seats >= query.NumberOfGuests && t.IsActive == true)
                .ToList();
            var tableIds = suitableTables.Select(t => t.Id).ToList();
            var searchEnd = query.ReservationStart.Date.AddDays(7);
            var existingReservations = _context.Reservations
                .Where(r => tableIds.Contains(r.TableId) &&
                            r.ReservationEnd > query.ReservationStart &&
                            r.ReservationStart < searchEnd)
                .OrderBy(r => r.ReservationStart)
                .ToList();
            for (int dayOffset = 0; dayOffset < 7; dayOffset++)
            {
                var nextAvailableReservations = new List<NextAvailableReservationDto>();
                var currentDate = query.ReservationStart.Date.AddDays(dayOffset);

                var openingHour =
                    _openingHoursService.GetOpeningHours(currentDate);

                if (openingHour == null ||
                    openingHour.IsClosed ||
                    openingHour.OpenTime == null ||
                    openingHour.CloseTime == null)
                {
                    continue;
                }

                var openingDateTime =
                    currentDate + openingHour.OpenTime.Value.ToTimeSpan();

                var closingDateTime =
                    currentDate + openingHour.CloseTime.Value.ToTimeSpan();

                var searchStart = dayOffset == 0
                    ? (query.ReservationStart < openingDateTime
                        ? openingDateTime
                        : query.ReservationStart)
                    : openingDateTime;


                foreach (var table in suitableTables)
                {

                    var reservationsForTable = existingReservations
                    .Where(r => r.TableId == table.Id);

                    var nextAvailableStart = _availabilityCalculator.FindNextAvailableStart(searchStart, duration, reservationsForTable);

                    if (nextAvailableStart + duration > closingDateTime)
                    {
                        continue;
                    }
                    var nextAvailableReservation = new NextAvailableReservationDto
                    {
                        TableId = table.Id,
                        TableNumber = table.TableNumber,
                        Seats = table.Seats,
                        ReservationStart = nextAvailableStart,
                        ReservationEnd = nextAvailableStart + duration
                    };
                    nextAvailableReservations.Add(nextAvailableReservation);

                }
                if (nextAvailableReservations.Count > 0)
                {
                    return nextAvailableReservations
                        .OrderBy(r => r.ReservationStart)
                        .First();
                }
            }
            return null;
        }
        

        private bool IsTableAvailable(int tableId, DateTime reservationStart, DateTime reservationEnd, int? excludedReservationId = null)
        {
            var hasConflict = _context.Reservations.Any(r => r.TableId == tableId && r.Id != excludedReservationId &&
                reservationStart < r.ReservationEnd &&
                reservationEnd > r.ReservationStart);
            return !hasConflict;
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
            if (!IsTableAvailable(existingReservation.TableId, updatedReservation.ReservationStart, updatedReservation.ReservationEnd, existingReservation.Id))
            {
                return UpdateReservationResult.TableAlreadyReserved;
            }
            if (!_openingHoursService.IsRestaurantOpen(updatedReservation.ReservationStart, updatedReservation.ReservationEnd))
            {
                return UpdateReservationResult.RestaurantClosed;
            }
            if (!IsThereEnoughSeats(existingReservation.Table.Seats, updatedReservation.NumberOfGuests))
            {
                return UpdateReservationResult.NotEnoughSeats;
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
