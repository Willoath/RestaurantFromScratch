using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;
using RestaurantFromScratch.Data;
using RestaurantFromScratch.Dtos;
using RestaurantFromScratch.Models;
using RestaurantFromScratch.Services;


namespace RestaurantApi.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public void GetNextAvailableReservation_WhenCurrentDayIsFull_ReturnsNextDay()
        {
            // Arrange
            using var connection =
                new SqliteConnection("Filename=:memory:");

            connection.Open();

            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new RestaurantContext(options);

            context.Database.EnsureCreated();

            var table = new Table
            {
                TableNumber = 1,
                Seats = 4,
                IsActive = true
            };

            context.Tables.Add(table);
            context.SaveChanges();

            context.OpeningHours.AddRange(
                new OpeningHour
                {
                    DayOfWeek = DayOfWeek.Monday,
                    OpenTime = new TimeOnly(12, 0),
                    CloseTime = new TimeOnly(22, 0),
                    IsClosed = false
                },
                new OpeningHour
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    OpenTime = new TimeOnly(12, 0),
                    CloseTime = new TimeOnly(22, 0),
                    IsClosed = false
                });

            context.Reservations.Add(
                new Reservation
                {
                    CustomerName = "Test",
                    PhoneNumber = "111111111",
                    NumberOfGuests = 4,
                    TableId = table.Id,
                    ReservationStart =
                        new DateTime(2026, 11, 16, 18, 0, 0),
                    ReservationEnd =
                        new DateTime(2026, 11, 16, 22, 0, 0)
                });

            context.SaveChanges();

            var openingHoursService =
                new OpeningHoursService(context);

            var calculator =
                new ReservationAvailabilityCalculator();

            var reservationService =
                new ReservationService(
                    context,
                    openingHoursService,
                    calculator);

            var query = new AvailableTablesQueryDto
            {
                ReservationStart =
                    new DateTime(2026, 11, 16, 18, 0, 0),

                ReservationEnd =
                    new DateTime(2026, 11, 16, 20, 0, 0),

                NumberOfGuests = 4
            };

            var expectedStart =
                new DateTime(2026, 11, 17, 12, 0, 0);

            var expectedEnd =
                new DateTime(2026, 11, 17, 14, 0, 0);

            // Act
            var result =
                reservationService.GetNextAvailableReservation(query);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(expectedStart, result!.ReservationStart);
            Assert.Equal(expectedEnd, result.ReservationEnd);
            Assert.Equal(table.Id, result.TableId);
        }
        [Fact]
        public void GetNextAvailableReservation_WhenNextDayIsClosed_ReturnsFollowingOpenDay()
        {
            // Arrange
            using var connection =
                new SqliteConnection("Filename=:memory:");

            connection.Open();

            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new RestaurantContext(options);

            context.Database.EnsureCreated();

            var table = new Table
            {
                TableNumber = 1,
                Seats = 4,
                IsActive = true
            };

            context.Tables.Add(table);
            context.SaveChanges();

            context.OpeningHours.AddRange(
                new OpeningHour
                {
                    DayOfWeek = DayOfWeek.Monday,
                    OpenTime = new TimeOnly(12, 0),
                    CloseTime = new TimeOnly(22, 0),
                    IsClosed = false
                },
                new OpeningHour
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    OpenTime = null,
                    CloseTime = null,
                    IsClosed = true
                },
                new OpeningHour
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    OpenTime = new TimeOnly(12, 0),
                    CloseTime = new TimeOnly(22, 0),
                    IsClosed = false
                });

            context.Reservations.Add(
                new Reservation
                {
                    CustomerName = "Test",
                    PhoneNumber = "111111111",
                    NumberOfGuests = 4,
                    TableId = table.Id,
                    ReservationStart =
                        new DateTime(2026, 11, 16, 18, 0, 0),
                    ReservationEnd =
                        new DateTime(2026, 11, 16, 22, 0, 0)
                });

            context.SaveChanges();

            var openingHoursService =
                new OpeningHoursService(context);

            var calculator =
                new ReservationAvailabilityCalculator();

            var reservationService =
                new ReservationService(
                    context,
                    openingHoursService,
                    calculator);

            var query = new AvailableTablesQueryDto
            {
                ReservationStart =
                    new DateTime(2026, 11, 16, 18, 0, 0),

                ReservationEnd =
                    new DateTime(2026, 11, 16, 20, 0, 0),

                NumberOfGuests = 4
            };

            var expectedStart =
                new DateTime(2026, 11, 18, 12, 0, 0);

            var expectedEnd =
                new DateTime(2026, 11, 18, 14, 0, 0);

            // Act
            var result =
                reservationService.GetNextAvailableReservation(query);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(
                expectedStart,
                result!.ReservationStart);

            Assert.Equal(
                expectedEnd,
                result.ReservationEnd);

            Assert.Equal(
                table.Id,
                result.TableId);
        }
        [Fact]
        public void GetNextAvailableReservation_WhenNoAvailabilityForSevenDays_ReturnsNull()
        {
            // Arrange
            using var connection =
                new SqliteConnection("Filename=:memory:");

            connection.Open();

            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new RestaurantContext(options);

            context.Database.EnsureCreated();

            var table = new Table
            {
                TableNumber = 1,
                Seats = 4,
                IsActive = true
            };

            context.Tables.Add(table);
            context.SaveChanges();

            foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
            {
                context.OpeningHours.Add(
                    new OpeningHour
                    {
                        DayOfWeek = day,
                        OpenTime = new TimeOnly(12, 0),
                        CloseTime = new TimeOnly(22, 0),
                        IsClosed = false
                    });
            }

            var firstDay =
                new DateTime(2026, 11, 16);

            for (int i = 0; i < 7; i++)
            {
                var date = firstDay.AddDays(i);

                context.Reservations.Add(
                    new Reservation
                    {
                        CustomerName = $"Test {i}",
                        PhoneNumber = "111111111",
                        NumberOfGuests = 4,
                        TableId = table.Id,

                        ReservationStart =
                            date.AddHours(12),

                        ReservationEnd =
                            date.AddHours(22)
                    });
            }

            context.SaveChanges();

            var openingHoursService =
                new OpeningHoursService(context);

            var calculator =
                new ReservationAvailabilityCalculator();

            var reservationService =
                new ReservationService(
                    context,
                    openingHoursService,
                    calculator);

            var query = new AvailableTablesQueryDto
            {
                ReservationStart =
                    new DateTime(2026, 11, 16, 18, 0, 0),

                ReservationEnd =
                    new DateTime(2026, 11, 16, 20, 0, 0),

                NumberOfGuests = 4
            };

            // Act
            var result =
                reservationService.GetNextAvailableReservation(query);

            // Assert
            Assert.Null(result);
        }
    }
}
