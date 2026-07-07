using Microsoft.EntityFrameworkCore;
using RestaurantFromScratch.Models;
namespace RestaurantFromScratch.Data
{
    public class RestaurantContext : DbContext
    {
        DbSet<Reservation> Reservations { get; set; } = null!;
        DbSet<Table> Tables { get; set; } = null!;
        DbSet<MenuItem> MenuItems { get; set; } = null!;
    }
    
}
