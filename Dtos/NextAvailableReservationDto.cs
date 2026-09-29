namespace RestaurantFromScratch.Dtos
{
    public class NextAvailableReservationDto
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int Seats { get; set; }

        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
    }
}
