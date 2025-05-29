using Microsoft.EntityFrameworkCore;
using Restaurant;

namespace RestaurantReservation
{
    public class ReservationContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<SeedInfo> SeedInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=reservations.db");
        }

    }
}