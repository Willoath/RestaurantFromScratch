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
        public ActionResult AddReservation([FromBody] Reservation reservation)
        {
            var result = _reservationService.AddReservation(reservation);
            if (result == AddReservationResult.TableNotFound)
            {
                return NotFound();
            }
            if (result == AddReservationResult.TableAlreadyReserved)
            {
                return Conflict("The table is already reserved for the selected time.");
            }
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateReservation(int id, [FromBody] UpdateReservationDto updatedReservation)
        {
            if (!_reservationService.UpdateReservation(updatedReservation, id))
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
