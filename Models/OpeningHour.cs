namespace RestaurantApi.Models
{
    public class OpeningHour
    {
        
            public int Id { get; set; }

            public DayOfWeek DayOfWeek { get; set; }

            public TimeOnly? OpenTime { get; set; }

            public TimeOnly? CloseTime { get; set; }

            public bool IsClosed { get; set; }
        
    }
}
