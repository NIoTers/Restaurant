using System;
using System.Collections.Generic;
using System.Globalization;

namespace RestaurantReservation
{
    public class AvailabilityManager
    {
        private readonly Dictionary<DateTime, BookingStatus[]> availability
            = new Dictionary<DateTime, BookingStatus[]>();

        public static readonly string[] TimeSlots = {
            "1:00 PM", "5:00 PM", "9:00 PM",
            "1:00 AM", "5:00 AM", "9:00 AM"
        };

        public void InitializeAvailability(DateTime start, DateTime end)
        {
            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                if (availability.ContainsKey(date)) continue;
                var slots = new BookingStatus[TimeSlots.Length];
                for (int i = 0; i < slots.Length; i++)
                    slots[i] = BookingStatus.Open;
                availability[date] = slots;
            }
        }

        public bool TryParseDate(string input, DateTime date)
        {
            string[] formats = new string[] {
        "MM/dd/yy", "M/d/yy", "MMMM d, yyyy", "MMM d, yyyy", "MM/dd/yyyy", "M/d/yyyy",
        "MM-dd-yy", "M-d-yy", "MM-dd-yyyy", "M-d-yyyy"
    };

            foreach (var format in formats)
            {
                try
                {
                    date = DateTime.ParseExact(input, format, null);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public bool IsValidDate(DateTime date)
        {
            var today = DateTime.Today;
            var maxDate = new DateTime(2026, 6, 30);
            return date.Date >= today && date.Date <= maxDate;
        }

        public bool IsSlotAvailable(DateTime date, int slotIndex)
        {
            if (!availability.ContainsKey(date.Date))
                return false;
            return availability[date.Date][slotIndex] == BookingStatus.Open;
        }

        public void MarkSlotAsFull(DateTime date, int slotIndex)
        {
            if (availability.ContainsKey(date.Date))
                availability[date.Date][slotIndex] = BookingStatus.Full;
        }


        public void RandomizeReservations(DateTime start, DateTime end)
        {
            Random rand = new Random();
            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                if (!availability.ContainsKey(date)) continue;

                for (int i = 0; i < TimeSlots.Length; i++)
                {
                    if (rand.NextDouble() <= 0.5)
                    {
                        availability[date][i] = BookingStatus.Full;
                    }
                }
            }
        }

        public void MarkSlotAsAvailable(DateTime date, int slotIndex)
        {
            if (availability.ContainsKey(date.Date))
                availability[date.Date][slotIndex] = BookingStatus.Open;
        }

        public void ShowAvailability(DateTime date)
        {
            if (!availability.ContainsKey(date))
            {
                Console.WriteLine("No availability data for this date.");
                return;
            }

            var slots = availability[date];
            Console.WriteLine($"Availability for {date.ToString("MMMM d, yyyy")}:");

            for (int i = 0; i < TimeSlots.Length; i++)
            {
                string status = slots[i] == BookingStatus.Full ? "Reserved" : "Available";

                if (slots[i] == BookingStatus.Full)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{i + 1}. {TimeSlots[i]} - {status}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i + 1}. {TimeSlots[i]} - {status}");
                }
            }

            Console.ResetColor();
        }

    }
}
