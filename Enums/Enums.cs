namespace RestaurantFromScratch.Enums
{

    public enum Category
    {
        Appetizer,
        MainCourse,
        Dessert,
        Beverage
    }
    public enum AddReservationResult
    {
        Success,
        TableNotFound,
        TableAlreadyReserved,
        NotEnoughSeats
    }
    public enum UpdateReservationResult
    {
        Success,
        ReservationNotFound,
        TableAlreadyReserved,
        NotEnoughSeats
    }
}
