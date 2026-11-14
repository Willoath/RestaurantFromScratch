using System;
using System.Collections.Generic;
using System.Text;
using RestaurantFromScratch.Models;
using RestaurantFromScratch.Services;

namespace RestaurantApi.Tests
{
    public class ReservationAvailabilityCalculatorTests
    {
        [Fact]
        public void FindNextAvailableStart_WhenRequestedTimeIsFree_ReturnsRequestedStart()
        {
            // Arrange
            var calculator = new ReservationAvailabilityCalculator();

            var requestedStart = new DateTime(2026, 10, 10, 18, 0, 0);
            var duration = TimeSpan.FromHours(2);

            var reservations = new List<Reservation>();

            // Act
            var result = calculator.FindNextAvailableStart(
                requestedStart,
                duration,
                reservations);

            // Assert
            Assert.Equal(requestedStart, result);
        }
        [Fact]
        public void FindNextAvailableStart_WhenMultipleReservationsConflict_ReturnsFirstFreeTime()
        {
            // Arrange
            var calculator = new ReservationAvailabilityCalculator();

            var requestedStart = new DateTime(2026, 10, 10, 18, 0, 0);
            var duration = TimeSpan.FromHours(2);

            var reservations = new List<Reservation>{
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 17, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 18, 30, 0)},
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 19, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 21, 0, 0)},
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 22, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 23, 0, 0)}
            };

            var expected = new DateTime(2026, 10, 10, 23, 0, 0);

            // Act
            var result = calculator.FindNextAvailableStart(
                requestedStart,
                duration,
                reservations);

            // Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void FindNextAvailableStart_WhenRequestedTimeStartsAtExistingReservationEnd_ReturnsRequestedStart()
        {
            // Arrange
            var calculator = new ReservationAvailabilityCalculator();

            var requestedStart = new DateTime(2026, 10, 10, 18, 0, 0);
            var duration = TimeSpan.FromHours(2);

            var reservations = new List<Reservation>{
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 16, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 18, 0, 0)}
            };

            // Act
            var result = calculator.FindNextAvailableStart(
                requestedStart,
                duration,
                reservations);

            // Assert
            Assert.Equal(requestedStart, result);
        }
        [Fact]
        public void FindNextAvailableStart_WhenRequestedTimeEndsAtNextReservationStart_ReturnsRequestedStart()
        {
            // Arrange
            var calculator = new ReservationAvailabilityCalculator();

            var requestedStart = new DateTime(2026, 10, 10, 18, 0, 0);
            var duration = TimeSpan.FromHours(2);

            var reservations = new List<Reservation>{
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 20, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 22, 0, 0)}
            };

            // Act
            var result = calculator.FindNextAvailableStart(
                requestedStart,
                duration,
                reservations);

            // Assert
            Assert.Equal(requestedStart, result);
        }
        [Fact]
        public void FindNextAvailableStart_WhenRequestedTimeStartsSimultaneouslyWithReservationStart_ReturnsFirstFreeTime()
        {
            // Arrange
            var calculator = new ReservationAvailabilityCalculator();

            var requestedStart = new DateTime(2026, 10, 10, 18, 0, 0);
            var duration = TimeSpan.FromHours(2);

            var reservations = new List<Reservation>{
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 18, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 20, 0, 0)}
            };

            var expected = new DateTime(2026, 10, 10, 20, 0, 0);

            // Act
            var result = calculator.FindNextAvailableStart(
                requestedStart,
                duration,
                reservations);

            // Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void FindNextAvailableStart_WhenReservationsAreNotInOrder_ReturnsFirstFreeTime()
        {
            // Arrange
            var calculator = new ReservationAvailabilityCalculator();

            var requestedStart = new DateTime(2026, 10, 10, 18, 0, 0);
            var duration = TimeSpan.FromHours(2);

            var reservations = new List<Reservation>{
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 19, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 21, 0, 0)}, 
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 17, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 18, 30, 0)},
            new Reservation{
            ReservationStart = new DateTime(2026, 10, 10, 22, 0, 0),
            ReservationEnd = new DateTime(2026, 10, 10, 23, 0, 0)}
            };

            var expected = new DateTime(2026, 10, 10, 23, 0, 0);

            // Act
            var result = calculator.FindNextAvailableStart(
                requestedStart,
                duration,
                reservations);

            // Assert
            Assert.Equal(expected, result);
        }
    }


}
