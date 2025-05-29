using System.Collections.Generic;
using System.Linq;

namespace RestaurantReservation
{
    public class ReservationSystemDbAdapter
    {
        public List<Reservation> LoadReservations()
        {
            using (var db = new ReservationContext())
            {
                return db.Reservations.ToList();
            }
        }

        public void SaveReservation(Reservation reservation)
        {
            using (var db = new ReservationContext())
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
            }
        }

        public void RemoveReservation(string referenceId)
        {
            using (var db = new ReservationContext())
            {
                var reservation = db.Reservations.FirstOrDefault(r => r.ReferenceId == referenceId);
                if (reservation != null)
                {
                    db.Reservations.Remove(reservation);
                    db.SaveChanges();
                }
            }
        }

        public bool IsSlotReserved(int year, int monthIndex, int day, int timeIndex)
        {
            using (var db = new ReservationContext())
            {
                return db.Reservations.Any(r =>
                    r.Year == year &&
                    r.MonthIndex == monthIndex &&
                    r.Day == day &&
                    r.TimeIndex == timeIndex
                );
            }
        }
    }
}