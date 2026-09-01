using Microsoft.AspNetCore.Mvc;
using RestaurantFromScratch.Services;
using RestaurantFromScratch.Models;
using RestaurantFromScratch.Enums;
using RestaurantFromScratch.Dtos;

namespace RestaurantFromScratch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public List<Reservation> GetAllReservations()
        {
            return _reservationService.GetAllReservations();
        }
        [HttpGet("{id}")]
        public ActionResult<Reservation> GetReservationById(int id)
        {
            var reservation = _reservationService.GetReservationById(id);

            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult AddReservation([FromBody] CreateReservationDto reservation)
        {
            var result = _reservationService.AddReservation(reservation);
            if (result.Result == AddReservationResult.TableNotFound)
            {
                return NotFound();
            }
            if (result.Result == AddReservationResult.TableAlreadyReserved)
            {
                return Conflict("The table is already reserved for the selected time.");
            }
            return CreatedAtAction(nameof(GetReservationById), new { id = result.ReservationId }, new { id = result.ReservationId });
        }
        [HttpPut("{id}")]
        public ActionResult UpdateReservation(int id, [FromBody] UpdateReservationDto updatedReservation)
        {
            var result = _reservationService.UpdateReservation(updatedReservation, id);
            if (result == UpdateReservationResult.ReservationNotFound)
            {
                return NotFound();
            }
            if (result == UpdateReservationResult.TableAlreadyReserved)
            {
                return Conflict("The table is already reserved for the selected time.");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteReservation(int id)
        {
            var deleted = _reservationService.DeleteReservation(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
