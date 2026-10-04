using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;
using RestaurantFromScratch.Models;
namespace RestaurantFromScratch.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options): base(options){}
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Table> Tables { get; set; } = null!;
        public DbSet<MenuItem> MenuItems { get; set; } = null!;
        public DbSet<OpeningHour> OpeningHours { get; set; }
    }
    
}
