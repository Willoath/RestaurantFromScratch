namespace RestaurantFromScratch.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ReservationStart { get; set; }

        public int NumberOfGuests { get; set; }
        public DateTime ReservationEnd { get; set; }
        public string? Notes { get; set; }

        public int TableId { get; set; }

        public int TableNumber { get; set; }

        public int TableSeats { get; set; }
    }
}
