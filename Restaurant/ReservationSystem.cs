using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantReservation
{
    public class ReservationSystem
    {
        private MenuManager menuManager = new MenuManager();
        private Scheduler availabilityManager = new Scheduler();
        private List<Reservation> reservations = new List<Reservation>();
        static string divider = "*******************************************";

        public ReservationSystem()
        {
            availabilityManager.InitializeAvailability(DateTime.Today, new DateTime(2026, 6, 30));
            availabilityManager.RandomizeReservations(DateTime.Today, new DateTime(2026, 6, 30));
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                int selected = menuManager.ShowMainMenu();
                switch (selected)
                {
                    case 0:
                        menuManager.ViewMenu();
                        break;
                    case 1:
                        MakeReservation();
                        break;
                    case 2:
                        SearchReservation();
                        break;
                    case 3:
                        CancelReservation();
                        break;
                    case 4:
                        AboutUs();
                        break;
                    case 5:
                        Console.WriteLine("👋 Exiting... Have a great day!");
                        running = false;
                        break;
                }
            }
        }

        private void AboutUs()
        {
            Console.Clear();
            Console.WriteLine("ℹ️ About Us\n");
            Console.WriteLine("The M.A.R.I.L.A.G. Reservation System was proudly developed by the following team:\n");
            Console.WriteLine("🫃🏻 Cholo H. Gallardo            - Team Leader / Main Developer");
            Console.WriteLine("👨‍💻 Charles Andrei S. Alarcon    - Make Reservation / Code Optimization");
            Console.WriteLine("🎨 Princess Naoebe P. Dizon     - UI Design & Documentation");
            Console.WriteLine("📆 Joseph Andrew C. Fernandez   - Search Schedule Module");
            Console.WriteLine("📦 Adrian Kyle C. Garfin        - View Packages Module");
            Console.WriteLine("📝 Gwyneth B. Saga              - Cancel Reservation Module");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        private void MakeReservation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("           📝 Make a Reservation          ");
            Console.WriteLine(divider);
            Console.ResetColor();

            string name;
            while (true)
            {
                Console.Write("👤 Name: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠ Name cannot be blank.");
                Console.ResetColor();
            }

            string contact;
            while (true)
            {
                Console.Write("📞 Contact Number: ");
                contact = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(contact) && contact.Length >= 7 && contact.All(char.IsDigit)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠ Contact must be at least 7 digits and digits only.");
                Console.ResetColor();
            }

            int existing = reservations.Count(r =>
                r.Contact == contact && r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing >= 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠ You have reached the maximum of 5 reservations.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            int tables;
            while (true)
            {
                Console.Write("🪑 Number of Tables (1-3): ");
                if (int.TryParse(Console.ReadLine(), out tables) && tables > 0 && tables <= 3) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠ Invalid number of tables. Please enter a number between 1 and 3.");
                Console.ResetColor();
            }

            int guests;
            while (true)
            {
                Console.Write("👥 Number of Guests: ");
                if (int.TryParse(Console.ReadLine(), out guests))
                {
                    int minGuests = tables == 1 ? 0 : tables + 1;
                    int maxGuests = tables * 2;
                    if ((tables == 1 && guests == 0) || (guests >= minGuests && guests <= maxGuests)) break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"⚠ Guests must be between {(tables == 1 ? 0 : tables + 1)} and {tables * 2}.");
                Console.ResetColor();
            }

            DateTime reservationDate;
            while (true)
            {
                Console.Write("📅 Enter date (MM/DD/YY or Month d, yyyy): ");
                string input = Console.ReadLine();
                try
                {
                    reservationDate = DateTime.Parse(input);
                    if (reservationDate >= DateTime.Today)
                    {
                        bool allFull = true;
                        for (int i = 0; i < Scheduler.TimeSlots.Length; i++)
                        {
                            if (availabilityManager.IsSlotAvailable(reservationDate, i))
                            {
                                allFull = false;
                                break;
                            }
                        }
                        if (allFull)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("⚠ All time slots are fully booked for this date. Please choose another date.");
                            Console.ResetColor();
                            Console.ReadKey();
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("⚠ The reservation date cannot be in the past. Please select a valid future date.");
                        Console.ResetColor();
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ Invalid or out-of-range date. Please try again.");
                    Console.ResetColor();
                }
            }

            int timeIndex;
            while (true)
            {
                Console.Clear();
                availabilityManager.ShowAvailability(reservationDate);
                Console.Write("Select time slot number: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out timeIndex)
                    && timeIndex >= 1 && timeIndex <= Scheduler.TimeSlots.Length
                    && availabilityManager.IsSlotAvailable(reservationDate, timeIndex - 1))
                {
                    timeIndex -= 1;
                    break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠ Invalid or unavailable slot. Please choose again.");
                Console.ResetColor();
                Console.ReadKey();
            }
            availabilityManager.MarkSlotAsFull(reservationDate, timeIndex);

            var package = menuManager.SelectPackage();
            var venue = menuManager.SelectDiningArea();

            var extraItems = new Dictionary<MenuItem, int>();
            int extraTotal = 0;
            Console.Clear();
            while (true)
            {
                Console.Write("➕ Add extra item? (y/n): ");
                var ans = Console.ReadLine().Trim().ToLower();
                if (ans == "n") break;
                if (ans != "y") continue;
                var allItems = menuManager.GetAllIndividualItems();
                Console.Clear();
                for (int i = 0; i < allItems.Count; i++)
                    Console.WriteLine($"{i + 1}. {allItems[i].Name} - {allItems[i].Price} PHP");
                Console.Write("\nItem number: ");
                if (int.TryParse(Console.ReadLine(), out int choice)
                    && choice >= 1 && choice <= allItems.Count)
                {
                    var item = allItems[choice - 1];
                    Console.Write("Quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                    {
                        if (!extraItems.ContainsKey(item)) extraItems[item] = 0;
                        extraItems[item] += qty;
                        extraTotal += item.Price * qty;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✔ {item.Name} × {qty} added.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("⚠ Invalid quantity.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ Invalid selection.");
                    Console.ResetColor();
                }
                Console.ReadKey();
            }

            var reservation = new Reservation
            {
                Name = name,
                Contact = contact,
                Tables = tables,
                Guests = guests,
                ReferenceId = "RES-" + new Random().Next(1000, 9999),
                PackageName = package.Item1,
                PackagePrice = package.Item2,
                DiningArea = venue.Item1,
                DiningPrice = venue.Item2,
                ExtraItems = extraItems.SelectMany(kv => Enumerable.Repeat(kv.Key, kv.Value)).ToList(),
                ExtraTotal = extraTotal,
                MonthIndex = reservationDate.Month - 1,
                Day = reservationDate.Day,
                Year = reservationDate.Year,
                TimeIndex = timeIndex
            };

            double discount = 0;
            string discountDetails = "No discount applied";
            if (tables == 1 && guests == 0)
            {
                Console.Write("⚖️ Would you like to apply the PWD discount? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    discount = 0.20d * package.Item2;
                    discountDetails = "PWD Discount - 20% Off";
                }
            }
            else if (guests > 0)
            {
                double perPersonCost = package.Item2 / (guests + 1);
                Console.Write("⚖️ Would you like to apply a discount? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.WriteLine("⚖️ Available discounts:");
                    Console.WriteLine("1. PWD - 20% Off");
                    Console.WriteLine("2. Child (1-7 years) - 50% Off");
                    Console.WriteLine("3. Infant - 100% Off");
                    Console.Write("Select discount (1/2/3): ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            discount = 0.20d * perPersonCost;
                            discountDetails = "PWD Discount - 20% Off";
                            break;
                        case "2":
                            discount = 0.50d * perPersonCost;
                            discountDetails = "Child Discount - 50% Off";
                            break;
                        case "3":
                            discount = perPersonCost;
                            discountDetails = "Infant Discount - 100% Off";
                            break;
                    }
                }
            }

            double subtotal = package.Item2 + venue.Item2 + extraTotal;
            double totalWithDiscount = subtotal - discount;
            double tax = totalWithDiscount * 0.12;
            double finalTotal = totalWithDiscount + tax;

            reservation.DiscountAmount = discount;
            reservation.TaxAmount = tax;
            reservation.FinalTotal = finalTotal;

            Console.Clear();
            Console.WriteLine(divider);
            Console.WriteLine("          📝 Reservation Receipt                  ");
            Console.WriteLine(divider);
            Console.WriteLine($"📅 Date     : {reservationDate:MMMM dd, yyyy}");
            Console.WriteLine($"⏰ Time     : {Scheduler.TimeSlots[timeIndex]}");
            Console.WriteLine($"🍽  Dining : {venue.Item1} - {venue.Item2:N2} PHP");
            Console.WriteLine($"📦 Package  : {package.Item1} - {package.Item2:N2} PHP");
            if (extraItems.Any())
            {
                Console.WriteLine("\n🧾 Extras:");
                foreach (var kv in extraItems)
                    Console.WriteLine($"• {kv.Key.Name} × {kv.Value} - {kv.Key.Price * kv.Value} PHP");
            }
            Console.WriteLine(divider);
            Console.WriteLine($"💰 Subtotal : {subtotal:N2} PHP");
            Console.WriteLine($"💸 Discount : {discount:N2} PHP ({discountDetails})");
            Console.WriteLine($"💰 Tax      : {tax:N2} PHP");
            Console.WriteLine($"💰 Total    : {finalTotal:N2} PHP");
            Console.WriteLine(divider);
            Console.WriteLine($"👤 Name     : {reservation.Name}");
            Console.WriteLine($"📞 Contact  : {reservation.Contact}");
            Console.WriteLine($"🪑 Tables   : {reservation.Tables}");
            Console.WriteLine($"👥 Guests   : {reservation.Guests}");
            Console.WriteLine($"🔖 Reference: {reservation.ReferenceId}");
            Console.WriteLine(divider);

            Console.Write("\n❓ Is all the above information correct? (y/n): ");
            var confirm = Console.ReadLine().Trim().ToLower();
            if (confirm != "y")
            {
                availabilityManager.MarkSlotAsAvailable(reservationDate, timeIndex);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n⚠ Reservation canceled. Let's start over.");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                MakeReservation();
                return;
            }

            reservations.Add(reservation);

            double payment = 0;
            while (true)
            {
                Console.Write($"\n💳 Amount due: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{finalTotal:N2}");
                Console.ResetColor();
                Console.Write(" PHP\n💳 Enter payment: ");
                if (double.TryParse(Console.ReadLine(), out payment) && Math.Round(payment, 2) >= Math.Round(finalTotal, 2)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Payment is invalid or less than the amount due.");
                Console.ResetColor();
            }
            double change = payment - finalTotal;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🔔 Payment accepted. Change: {change:N2} PHP");
            Console.ResetColor();

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
        private void SearchReservation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🔍 Search for a Reservation\n");

            string[] options = { "🔢 Receipt Number", "👤 Name", "📞 Contact", "🔙 Return" };
            int selected = menuManager.SelectMenu("Search Reservations", options);
            if (selected == 3) return;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🔍 Search for a Reservation\n");
            Console.ResetColor();
            Console.Write((selected == 0) ? "Enter Receipt Number: " :
                          (selected == 1) ? "Enter Name: " :
                                            "Enter Contact Number: ");
            string searchTerm = Console.ReadLine();

            if (selected == 2 && !searchTerm.All(char.IsDigit))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Contact number must contain digits only.");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            var results = reservations.Where(r =>
                (selected == 0 && r.ReferenceId.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (selected == 1 && r.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (selected == 2 && r.Contact.Equals(searchTerm))
            ).ToList();

            Console.Clear();

            if (results.Any())
            {
                foreach (var r in results)
                {
                    var date = new DateTime(r.Year, r.MonthIndex + 1, r.Day);
                    double extrasTotal = r.ExtraItems.Sum(e => e.Price);
                    double subtotal = r.PackagePrice + r.DiningPrice + extrasTotal;
                    double discount = r.DiscountAmount;
                    string discountDetails = (discount > 0) ? GetDiscountDetails(discount, r.PackagePrice, r.Guests) : "No discount applied";
                    double tax = r.TaxAmount;
                    double finalTotal = r.FinalTotal;

                    Console.WriteLine(divider);
                    Console.WriteLine("          📝 Reservation Receipt");
                    Console.WriteLine(divider);
                    Console.WriteLine($"📅 Date: {date:MMMM dd, yyyy}");
                    Console.WriteLine($"⏰ Time: {Scheduler.TimeSlots[r.TimeIndex]}");
                    Console.WriteLine($"🍽  Dining Area: {r.DiningArea} - {r.DiningPrice} PHP");
                    Console.WriteLine($"📦 Package: {r.PackageName} - {r.PackagePrice} PHP");

                    if (r.ExtraItems.Any())
                    {
                        Console.WriteLine("\n🧾 Extras:");
                        var grouped = r.ExtraItems.GroupBy(i => i.Name);
                        foreach (var g in grouped)
                            Console.WriteLine($"• {g.Key} × {g.Count()} - {g.First().Price * g.Count()} PHP");
                    }

                    Console.WriteLine(divider);
                    Console.WriteLine($"💰 Subtotal : {subtotal:0.00} PHP");
                    Console.WriteLine($"💸 Discount : {discount:0.00} PHP ({discountDetails})");
                    Console.WriteLine($"💰 Tax      : {tax:0.00} PHP");
                    Console.WriteLine($"💰 Total    : {finalTotal:0.00} PHP");
                    Console.WriteLine(divider);
                    Console.WriteLine($"👤 Name     : {r.Name}");
                    Console.WriteLine($"📞 Contact  : {r.Contact}");
                    Console.WriteLine($"🪑 Tables   : {r.Tables}");
                    Console.WriteLine($"👥 Guests   : {r.Guests}");
                    Console.WriteLine($"🔖 Reference: {r.ReferenceId}");
                    Console.WriteLine(divider);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ No reservations found.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }


        private string GetDiscountDetails(double discountAmount, double packagePrice, int guests)
        {
            double perPersonCost = packagePrice / (guests + 1);
            if (Math.Abs(discountAmount - (0.20 * packagePrice)) < 0.01)
                return "PWD Discount - 20% Off";
            if (Math.Abs(discountAmount - (0.20 * perPersonCost)) < 0.01)
                return "PWD Discount - 20% Off";
            if (Math.Abs(discountAmount - (0.50 * perPersonCost)) < 0.01)
                return "Child Discount - 50% Off";
            if (Math.Abs(discountAmount - perPersonCost) < 0.01)
                return "Infant Discount - 100% Off";
            return "Custom Discount";
        }


        private void CancelReservation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("         🔴 Cancel a Reservation         ");
            Console.WriteLine(divider);
            Console.ResetColor();

            Console.Write("🔍 Booking ID: ");
            string bookingId = Console.ReadLine();
            var foundReservation = reservations.FirstOrDefault(r => r.ReferenceId == bookingId);

            if (foundReservation != null)
            {
                DateTime reservationDate = new DateTime(foundReservation.Year, foundReservation.MonthIndex + 1, foundReservation.Day);

                if (reservationDate.Date <= DateTime.Today)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n⚠ You cannot cancel a reservation for today or a past date.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n" + divider);
                Console.WriteLine("         🎫 Reservation Details          ");
                Console.WriteLine(divider);
                Console.ResetColor();

                Console.WriteLine($"🔖 Reference  : {foundReservation.ReferenceId}");
                Console.WriteLine($"👤 Name       : {foundReservation.Name}");
                Console.WriteLine($"📞 Contact    : {foundReservation.Contact}");
                Console.WriteLine($"🪑 Tables     : {foundReservation.Tables}");
                Console.WriteLine($"👥 Guests     : {foundReservation.Guests}");
                Console.WriteLine($"📦 Package    : {foundReservation.PackageName}");
                Console.WriteLine($"💰 Total      : {foundReservation.Total:N2} PHP");
                Console.WriteLine($"📅 Date       : {reservationDate:MMMM dd, yyyy}");
                Console.WriteLine($"⏰ Time       : {Scheduler.TimeSlots[foundReservation.TimeIndex]}");
                Console.WriteLine(divider);

                Console.Write("\n⚠ Confirm cancellation? (y/n): ");
                string confirmCancel = Console.ReadLine().Trim().ToLower();

                if (confirmCancel == "y")
                {
                    double cancellationFee = 500.0;
                    double refundAmount = foundReservation.Total - cancellationFee;

                    reservations.Remove(foundReservation);
                    availabilityManager.MarkSlotAsAvailable(reservationDate, foundReservation.TimeIndex);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n" + divider);
                    Console.WriteLine("        ✅ Cancellation Complete         ");
                    Console.WriteLine(divider);
                    Console.ResetColor();

                    Console.WriteLine($"💸 Fee       : {cancellationFee:N2} PHP");
                    Console.WriteLine($"💰 Refund    : {refundAmount:N2} PHP");
                    Console.WriteLine(divider);
                }
                else
                {
                    Console.WriteLine("\n⚖️ Cancellation aborted.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ No reservation found with that Booking ID.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

    }
}
