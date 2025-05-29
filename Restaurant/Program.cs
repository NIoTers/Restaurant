using System;

namespace RestaurantReservation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ReservationContext())
            {
                db.Database.EnsureCreated();
            }
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var system = new ReservationSystem();
            system.Run();
        }
    }
}