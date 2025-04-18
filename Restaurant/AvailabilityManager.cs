using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantReservation
{
    public class AvailabilityManager
    {
        // Fields
        private Random rand = new Random();
        private Dictionary<int, Dictionary<int, BookingStatus[]>> availability = new Dictionary<int, Dictionary<int, BookingStatus[]>>();
        public Dictionary<string, Reservation> BookedReservations = new Dictionary<string, Reservation>();

        // Static Fields
        public static string[] Months = {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        public static string[] TimeSlots = {
            "1:00 PM", "5:00 PM", "9:00 PM", "1:00 AM", "5:00 AM", "9:00 AM"
        };

        // Fake Name Pools
        private List<string> firstNames = new List<string> {
            "Nora","Vilma","Fernando","Dolphy","Vice","Anne","Sarah","Daniel","Kathryn","Coco","Piolo","Liza","Enrique","Alden","Maine",
            "Kris","Toni","Angel","Bea","John Lloyd","Manny","Hidilyn","EJ","Margielyn","Carlos","June Mar","Alyssa","Kai","Marlon","Nesthy"
        };

        private List<string> lastNames = new List<string> {
            "Aunor","Santos","Poe Jr.","Quizon","Ganda","Curtis","Geronimo","Padilla","Bernardo","Martin","Pascual","Soberano","Gil",
            "Richards","Mendoza","Aquino","Fowler","Locsin","Alonzo","Cruz","Pacquiao","Diaz","Obiena","Didal","Yulo","Fajardo",
            "Valdez","Sotto","Tapales","Petecio"
        };

        // Main Initialization
        public void InitializeMonth(int monthIndex)
        {
            if (availability.ContainsKey(monthIndex)) return;

            Dictionary<int, BookingStatus[]> days = new Dictionary<int, BookingStatus[]>();
            int year = 2025;
            int daysInMonth = DateTime.DaysInMonth(year, monthIndex + 1);

            for (int day = 1; day <= daysInMonth; day++)
            {
                BookingStatus[] slots = new BookingStatus[TimeSlots.Length];
                for (int t = 0; t < TimeSlots.Length; t++)
                {
                    int r = rand.Next(3);
                    slots[t] = (r == 0) ? BookingStatus.Full : BookingStatus.Open;
                }
                days[day] = slots;
            }

            availability[monthIndex] = days;

            foreach (var day in days.Keys)
            {
                BookingStatus[] slots = days[day];
                for (int t = 0; t < slots.Length; t++)
                {
                    if (slots[t] == BookingStatus.Full)
                    {
                        string key = GenerateKey(monthIndex, day, t);
                        if (!BookedReservations.ContainsKey(key))
                        {
                            BookedReservations[key] = GenerateFakeReservation(monthIndex, day, t);
                        }
                    }
                }
            }
        }

        // Month Selection
        public int SelectMonth()
        {
            int selected = 0;
            Console.CursorVisible = false;
            DisplayMenu(selected);
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                int oldSelected = selected;

                if (key == ConsoleKey.UpArrow) selected = (selected == 0) ? Months.Length - 1 : selected - 1;
                else if (key == ConsoleKey.DownArrow) selected = (selected == Months.Length - 1) ? 0 : selected + 1;
                else if (key == ConsoleKey.Escape) return -1;
                else if (key == ConsoleKey.Enter) return selected;

                if (oldSelected != selected)
                {
                    UpdateMenuDisplay(oldSelected, selected);
                }
            }
        }

        private void DisplayMenu(int selected)
        {
            Console.Clear();
            Console.WriteLine("📅 Select a Month\n");
            for (int i = 0; i < Months.Length; i++)
            {
                Console.ForegroundColor = (i == selected) ? ConsoleColor.Yellow : ConsoleColor.Gray;
                Console.WriteLine((i == selected ? ">> " : "   ") + Months[i]);
            }
            Console.ResetColor();
            Console.WriteLine("\nEsc = Cancel | Enter = Select");
        }

        private void UpdateMenuDisplay(int oldSelected, int newSelected)
        {
            Console.SetCursorPosition(0, 2 + oldSelected);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("   " + Months[oldSelected]);
            Console.SetCursorPosition(0, 2 + newSelected);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">> " + Months[newSelected]);
            Console.ResetColor();
        }

        // Date Selection
        public int SelectDate(int monthIndex)
        {
            int year = 2025;
            int daysInMonth = DateTime.DaysInMonth(year, monthIndex + 1);
            int selectedDay = 1;
            int rows = (int)Math.Ceiling(daysInMonth / 2.0);

            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine($"📅 Select a Date ({Months[monthIndex]})\n");
            DrawCalendar(monthIndex, selectedDay);

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                int oldSelected = selectedDay;

                if (key == ConsoleKey.UpArrow && selectedDay - 1 > 0) selectedDay -= 1;
                else if (key == ConsoleKey.DownArrow && selectedDay + 1 <= daysInMonth) selectedDay += 1;
                else if (key == ConsoleKey.LeftArrow)
                {
                    int target = selectedDay - rows;
                    if (target >= 1) selectedDay = target;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    int target = selectedDay + rows;
                    if (target <= daysInMonth) selectedDay = target;
                }
                else if (key == ConsoleKey.Escape) return -1;
                else if (key == ConsoleKey.Enter) return selectedDay;

                if (oldSelected != selectedDay)
                {
                    UpdateCalendarHighlight(monthIndex, oldSelected, selectedDay);
                }
            }
        }

        private void DrawCalendar(int monthIndex, int selectedDay)
        {
            int year = 2025;
            int daysInMonth = DateTime.DaysInMonth(year, monthIndex + 1);
            int rows = (int)Math.Ceiling(daysInMonth / 2.0);

            for (int i = 1; i <= rows; i++)
            {
                int left = i;
                int right = i + rows;

                string leftStr = left <= daysInMonth ? FormatDayBox(monthIndex, left, left == selectedDay) : "";
                string rightStr = right <= daysInMonth ? FormatDayBox(monthIndex, right, right == selectedDay) : "";

                Console.WriteLine($"{leftStr,-20} {rightStr}");
            }

            Console.WriteLine("\n🔴 = Fully Booked  |  🟡 = Partially Booked | 🟢 = All Open");
            Console.WriteLine("Use arrow keys to move. Press Enter to select date, Esc to go back");
        }

        private void UpdateCalendarHighlight(int monthIndex, int oldDay, int newDay)
        {
            int year = 2025;
            int daysInMonth = DateTime.DaysInMonth(year, monthIndex + 1);
            int rows = (int)Math.Ceiling(daysInMonth / 2.0);

            int oldRow = (oldDay - 1) % rows;
            int oldCol = (oldDay - 1) / rows;
            int newRow = (newDay - 1) % rows;
            int newCol = (newDay - 1) / rows;

            Console.SetCursorPosition(oldCol * 21, 2 + oldRow);
            Console.Write(FormatDayBox(monthIndex, oldDay, false).PadRight(20));

            Console.SetCursorPosition(newCol * 21, 2 + newRow);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(FormatDayBox(monthIndex, newDay, true).PadRight(20));
            Console.ResetColor();
        }

        private string FormatDayBox(int monthIndex, int day, bool selected)
        {
            string marker = GetDayStatus(monthIndex, day);
            string prefix = selected ? ">>" : "  ";
            return $"{prefix} {day:D2} {marker}";
        }

        private string GetDayStatus(int monthIndex, int day)
        {
            BookingStatus[] slots = availability[monthIndex][day];
            int full = slots.Count(s => s == BookingStatus.Full);
            int open = slots.Count(s => s == BookingStatus.Open);
            if (full == slots.Length) return "🔴";
            if (open == slots.Length) return "🟢";
            return "🟡";
        }

        // Time Slot Selection
        public int SelectTimeSlot(int monthIndex, int day, bool isForViewing = false)
        {
            int selected = 0;
            BookingStatus[] slots = availability[monthIndex][day];
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine((isForViewing ? "📆 View Schedule" : "📆 Make Reservation") + "\n");
                Console.WriteLine("⏰ Select a Time Slot for " + Months[monthIndex] + " " + day + ":\n");

                int baseTop = Console.CursorTop;
                DrawAllSlots(slots, selected);

                Console.SetCursorPosition(0, baseTop + TimeSlots.Length + 1);
                Console.WriteLine("🔴 = Fully Booked  |  🟡 = Partial | 🟢 = Open");
                Console.WriteLine("Enter = " + (isForViewing ? "View Reservation" : "Select Slot") + " | Esc = Back");

                ConsoleKey key = Console.ReadKey(true).Key;
                int oldSelected = selected;

                if (key == ConsoleKey.UpArrow) selected = (selected == 0) ? TimeSlots.Length - 1 : selected - 1;
                else if (key == ConsoleKey.DownArrow) selected = (selected == TimeSlots.Length - 1) ? 0 : selected + 1;
                else if (key == ConsoleKey.Escape) return -1;
                else if (key == ConsoleKey.Enter)
                {
                    string keyStr = GenerateKey(monthIndex, day, selected);

                    if (isForViewing)
                    {
                        if (BookedReservations.ContainsKey(keyStr))
                        {
                            ShowReservation(BookedReservations[keyStr]);
                            continue;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nNo reservation found for this slot.");
                            Console.ResetColor();
                            Console.WriteLine("Press any key to return...");
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else
                    {
                        if (slots[selected] != BookingStatus.Full) return selected;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nThat slot is fully booked. Please choose another.");
                        Console.ResetColor();
                        Console.ReadKey();
                        continue;
                    }
                }

                if (oldSelected != selected)
                {
                    Console.SetCursorPosition(0, baseTop + oldSelected);
                    DrawSlot(slots, oldSelected, false);

                    Console.SetCursorPosition(0, baseTop + selected);
                    DrawSlot(slots, selected, true);
                }
            }
        }

        private void DrawAllSlots(BookingStatus[] slots, int selectedIndex)
        {
            for (int i = 0; i < TimeSlots.Length; i++)
            {
                DrawSlot(slots, i, i == selectedIndex);
            }
        }

        private void DrawSlot(BookingStatus[] slots, int index, bool selected)
        {
            string marker = slots[index] == BookingStatus.Full ? "🔴" :
                            slots[index] == BookingStatus.Open ? "🟢" : "🟡";
            string prefix = selected ? ">> " : "   ";
            string line = $"{prefix}{marker} {TimeSlots[index]}";
            Console.Write(line.PadRight(Console.WindowWidth));
        }

        private void DrawSingleTimeSlot(BookingStatus[] slots, int index, bool isSelected)
        {
            string marker = slots[index] == BookingStatus.Full ? "🔴" :
                            slots[index] == BookingStatus.Open ? "🟢" : "🟡";
            string prefix = isSelected ? ">>" : "  ";
            string line = $"{prefix} {marker} {TimeSlots[index]}";
            Console.Write(line.PadRight(Console.WindowWidth));
        }

        private void ShowReservation(Reservation res)
        {
            Console.Clear();
            Console.WriteLine("📋 Reservation Details:");
            Console.WriteLine($"📅 {Months[res.MonthIndex]} {res.Day} at {TimeSlots[res.TimeIndex]}");
            Console.WriteLine($"👤 Name: {res.Name}");
            Console.WriteLine($"📞 Contact: {res.Contact}");
            Console.WriteLine($"👥 Guests: {res.Guests}");
            Console.WriteLine($"📦 Package: {res.PackageName} - {res.PackagePrice} PHP");
            Console.WriteLine($"🍽 Dining Area: {res.DiningArea} - {res.DiningPrice} PHP");

            if (res.ExtraItems?.Count > 0)
            {
                Console.WriteLine("\n🧾 Additional Items:");
                var grouped = res.ExtraItems.GroupBy(i => i.Name)
                    .Select(g =>
                    {
                        var item = g.First();
                        int qty = g.Count();
                        return $"• {item.Name} × {qty} = {item.Price * qty} PHP";
                    });
                foreach (var line in grouped)
                    Console.WriteLine(line);
                Console.WriteLine($"➕ Extra Total: {res.ExtraTotal} PHP");
            }

            Console.WriteLine($"\n💰 Total: {res.Total} PHP");
            Console.WriteLine($"🔖 Reference ID: {res.ReferenceId}");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        public void ShowSchedule()
        {
            while (true)
            {
                int monthIndex = SelectMonth();
                if (monthIndex == -1) return;

                InitializeMonth(monthIndex);

                int year = 2025;
                int daysInMonth = DateTime.DaysInMonth(year, monthIndex + 1);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("📆 View Schedule\n");
                    Console.WriteLine($"📅 {Months[monthIndex]} Availability Schedule\n");
                    Console.WriteLine("Operating Hours: 1:00 PM → 9:00 AM\n");

                    for (int row = 1; row <= 15; row++)
                    {
                        int left = row;
                        int right = row + 15;

                        string leftStr = (left <= daysInMonth) ? FormatDateStatus(monthIndex, left) : "";
                        string rightStr = (right <= daysInMonth) ? FormatDateStatus(monthIndex, right) : "";

                        Console.WriteLine($"{leftStr,-25} {rightStr}");
                    }

                    Console.WriteLine("\n🔴 = Fully Booked  |  🟡 = Partially Booked | 🟢 = All Open");
                    Console.WriteLine("Enter = Select Date | Esc = Back");

                    int day = SelectDate(monthIndex);
                    if (day == -1) break;

                    int timeIndex = SelectTimeSlot(monthIndex, day, true);
                    if (timeIndex == -1) continue;
                }
            }
        }

        private string FormatDateStatus(int monthIndex, int day)
        {
            BookingStatus status = GetDateStatus(monthIndex, day);
            string marker = status == BookingStatus.Full ? "🔴" :
                            status == BookingStatus.Partial ? "🟡" : "🟢";
            string label = status == BookingStatus.Full ? "(Fully Booked)" :
                           status == BookingStatus.Partial ? "(Partial)" : "(Open)";
            return $"{marker}{day} {label}";
        }

        private BookingStatus GetDateStatus(int monthIndex, int day)
        {
            BookingStatus[] slots = availability[monthIndex][day];
            int full = slots.Count(s => s == BookingStatus.Full);
            int open = slots.Count(s => s == BookingStatus.Open);

            if (full == slots.Length) return BookingStatus.Full;
            if (open == slots.Length) return BookingStatus.Open;
            return BookingStatus.Partial;
        }

        public void MarkSlotAsFull(int monthIndex, int day, int timeIndex)
        {
            availability[monthIndex][day][timeIndex] = BookingStatus.Full;
        }

        private string GenerateKey(int month, int day, int time)
        {
            return $"{month}-{day}-{time}";
        }

        private Reservation GenerateFakeReservation(int month, int day, int time)
        {
            string first, last;
            do
            {
                first = firstNames[rand.Next(firstNames.Count)];
                last = lastNames[rand.Next(lastNames.Count)];
            } while (first == last);

            string fullName = first + " " + last;
            string reference = "RES-" + rand.Next(1000, 9999);
            Package package = MenuData.Packages[rand.Next(MenuData.Packages.Count)];

            string[] venues = { "Al Fresco", "Near Performer", "Dine-In" };
            int[] prices = { 20000, 22000, 18000 };
            int venueIndex = rand.Next(venues.Length);

            List<MenuItem> extraItems = new List<MenuItem>();
            int extraTotal = 0;
            int howManyExtras = rand.Next(0, 4);
            List<MenuItem> pool = new List<MenuItem>(MenuData.AllItems);
            for (int i = 0; i < howManyExtras; i++)
            {
                MenuItem item = pool[rand.Next(pool.Count)];
                int qty = rand.Next(1, 4);
                for (int q = 0; q < qty; q++)
                {
                    extraItems.Add(item);
                }
                extraTotal += item.Price * qty;
            }

            return new Reservation
            {
                Name = fullName,
                Contact = "09" + rand.Next(100000000, 999999999),
                Guests = rand.Next(2, 10),
                ReferenceId = reference,
                PackageName = package.Name,
                PackagePrice = package.TotalPrice,
                DiningArea = venues[venueIndex],
                DiningPrice = prices[venueIndex],
                ExtraItems = extraItems,
                ExtraTotal = extraTotal,
                MonthIndex = month,
                Day = day,
                TimeIndex = time
            };
        }
    }
}
