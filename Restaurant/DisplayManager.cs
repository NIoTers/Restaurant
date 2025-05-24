using System.Collections.Generic;
using System.Linq;
using System;

namespace RestaurantReservation
{
    public class DisplayManager
    {
        private static string divider = "═══════════════════════════════════════════";
        private static string smallDivider = "───────────────────────────────────────────";

        public void ClearConsole()
        {
            Console.Clear();
        }

        public void ShowAboutUs()
        {
            ClearConsole();
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

        public void ShowMakeReservationHeader()
        {
            ClearConsole();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("           📝 Make a Reservation          ");
            Console.WriteLine(divider);
            Console.ResetColor();
        }

        public void PromptName()
        {
            Console.Write("👤 Name: ");
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠ {message}");
            Console.ResetColor();
        }

        public void PromptContact()
        {
            Console.Write("📞 Contact Number: ");
        }

        public void ShowMaxReservationError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠ You have reached the maximum of 5 reservations.");
            Console.ResetColor();
            Console.ReadKey();
        }

        public void PromptTables()
        {
            Console.Write("🪑 Number of Tables (1-3): ");
        }

        public void PromptGuests()
        {
            Console.Write("👥 Number of Guests: ");
        }

        public void ShowGuestRangeError(int minGuests, int maxGuests)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠ Guests must be between {minGuests} and {maxGuests}.");
            Console.ResetColor();
        }

        public void PromptDate()
        {
            Console.Write("📅 Enter date (MM/DD/YY or Month d, yyyy): ");
        }

        public void ShowDateError(string errorType)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠ {errorType}");
            Console.ResetColor();
        }

        public void PromptTimeSlot()
        {
            Console.Write("Select time slot number: ");
        }

        public void PromptAddExtraItem()
        {
            Console.Write("➕ Add extra item? (y/n): ");
        }

        public void ShowExtraItems(List<MenuItem> items)
        {
            ClearConsole();
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{i + 1}. {items[i].Name} - {items[i].Price} PHP");
            Console.Write("\nItem number: ");
        }

        public void PromptQuantity()
        {
            Console.Write("Quantity: ");
        }

        public void ShowItemAdded(string itemName, int qty)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✔ {itemName} × {qty} added.");
            Console.ResetColor();
        }

        public void HandleNoMoreItems()
        {
            ClearConsole();
        }

        public void PromptDiscount(string type)
        {
            Console.Write($"⚖️ Would you like to apply {type}? (y/n): ");
        }

        public void ShowDiscountOptions()
        {
            Console.WriteLine("⚖️ Available discounts:");
            Console.WriteLine("1. PWD - 20% Off");
            Console.WriteLine("2. Child (1-7 years) - 50% Off");
            Console.WriteLine("3. Infant - 100% Off");
            Console.Write("Select discount (1/2/3): ");
        }

        public void ShowReservationPreview(Reservation reservation, DateTime reservationDate, Tuple<string, int> package, Tuple<string, int> venue)
        {
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("        🔍 Review Your Reservation        ");
            Console.WriteLine(divider);
            Console.ResetColor();
            Console.WriteLine($"👤 Name               : {reservation.Name}");
            Console.WriteLine($"📞 Contact            : {reservation.Contact}");
            Console.WriteLine($"🪑 Tables             : {reservation.Tables}");
            Console.WriteLine($"👥 Guests             : {reservation.Guests}");
            Console.WriteLine($"📅 Date               : {reservationDate:MMMM dd, yyyy}");
            Console.WriteLine($"⏰ Time               : {Scheduler.TimeSlots[reservation.TimeIndex]}");
            Console.WriteLine($"🍽  Dining Area        : {venue.Item1} - {venue.Item2:N2} PHP");
            Console.WriteLine($"📦 Package            : {package.Item1} - {package.Item2:N2} PHP");
            if (reservation.ExtraItems.Any())
            {
                Console.WriteLine("\n🧾 Extras:");
                var grouped = reservation.ExtraItems.GroupBy(i => i.Name);
                foreach (var g in grouped)
                    Console.WriteLine($"• {g.Key} × {g.Count()} - {g.First().Price * g.Count():N2} PHP");
            }
            Console.WriteLine(divider);
            Console.Write("❓ Is the information correct? (y/n): ");
        }

        public void ShowReservationReceipt(Reservation reservation, DateTime reservationDate, Tuple<string, int> package, Tuple<string, int> venue, double subtotal, double discount, string discountDetails, double tax, double finalTotal, double reservationFee)
        {
            ClearConsole();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("        📝 Reservation Receipt            ");
            Console.WriteLine(divider);
            Console.ResetColor();
            Console.WriteLine($"📅 Date               : {reservationDate:MMMM dd, yyyy}");
            Console.WriteLine($"⏰ Time               : {Scheduler.TimeSlots[reservation.TimeIndex]}");
            Console.WriteLine($"🍽  Dining Area        : {venue.Item1} - {venue.Item2:N2} PHP");
            Console.WriteLine($"📦 Package            : {package.Item1} - {package.Item2:N2} PHP");
            if (reservation.ExtraItems.Any())
            {
                Console.WriteLine("\n🧾 Extras:");
                var grouped = reservation.ExtraItems.GroupBy(i => i.Name);
                foreach (var g in grouped)
                    Console.WriteLine($"• {g.Key} × {g.Count()} - {g.First().Price * g.Count():N2} PHP");
            }
            Console.WriteLine(smallDivider);
            Console.WriteLine($"💰 Subtotal           : {subtotal:N2} PHP");
            Console.WriteLine($"💸 Discount           : {discount:N2} PHP ({discountDetails})");
            Console.WriteLine($"💰 Tax (12%)          : {tax:N2} PHP");
            Console.WriteLine($"💳 Reservation Fee    : {reservationFee:N2} PHP");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"💰 Total Amount Due   : {finalTotal:N2} PHP");
            Console.ResetColor();
            Console.WriteLine(smallDivider);
            Console.WriteLine($"👤 Name               : {reservation.Name}");
            Console.WriteLine($"📞 Contact            : {reservation.Contact}");
            Console.WriteLine($"🪑 Tables             : {reservation.Tables}");
            Console.WriteLine($"👥 Guests             : {reservation.Guests}");
            Console.WriteLine($"🔖 Reference          : {reservation.ReferenceId}");
            Console.WriteLine(divider);
            Console.WriteLine("ℹ️ Terms & Conditions:");
            Console.WriteLine("   By confirming you agree to:");
            Console.WriteLine("   • 500 PHP reservation fee (non-refundable on/after date)");
            Console.WriteLine("   • 500 PHP cancellation fee");
            Console.WriteLine("   • Cancellations only before reservation date");
            Console.Write("\nAccept and proceed? (y/n): ");
        }

        public void ShowReservationReceiptForPayment(Reservation reservation, DateTime reservationDate, Tuple<string, int> package, Tuple<string, int> venue, double subtotal, double discount, string discountDetails, double tax, double finalTotal, double reservationFee)
        {
            ClearConsole();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("        📝 Reservation Receipt            ");
            Console.WriteLine(divider);
            Console.ResetColor();
            Console.WriteLine($"📅 Date               : {reservationDate:MMMM dd, yyyy}");
            Console.WriteLine($"⏰ Time               : {Scheduler.TimeSlots[reservation.TimeIndex]}");
            Console.WriteLine($"🍽  Dining Area        : {venue.Item1} - {venue.Item2:N2} PHP");
            Console.WriteLine($"📦 Package            : {package.Item1} - {package.Item2:N2} PHP");
            if (reservation.ExtraItems.Any())
            {
                Console.WriteLine("\n🧾 Extras:");
                var grouped = reservation.ExtraItems.GroupBy(i => i.Name);
                foreach (var g in grouped)
                    Console.WriteLine($"• {g.Key} × {g.Count()} - {g.First().Price * g.Count():N2} PHP");
            }
            Console.WriteLine(smallDivider);
            Console.WriteLine($"💰 Subtotal           : {subtotal:N2} PHP");
            Console.WriteLine($"💸 Discount           : {discount:N2} PHP ({discountDetails})");
            Console.WriteLine($"💰 Tax (12%)          : {tax:N2} PHP");
            Console.WriteLine($"💳 Reservation Fee    : {reservationFee:N2} PHP");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"💰 Total Amount Due   : {finalTotal:N2} PHP");
            Console.ResetColor();
            Console.WriteLine(smallDivider);
            Console.WriteLine($"👤 Name               : {reservation.Name}");
            Console.WriteLine($"📞 Contact            : {reservation.Contact}");
            Console.WriteLine($"🪑 Tables             : {reservation.Tables}");
            Console.WriteLine($"👥 Guests             : {reservation.Guests}");
            Console.WriteLine($"🔖 Reference          : {reservation.ReferenceId}");
            Console.WriteLine(divider);
        }

        public void ShowReservationCanceled()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n⚠ Reservation canceled. Let's start over.");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void PromptPayment(double finalTotal)
        {
            Console.Write($"💳 Enter payment amount: ");
        }

        public void ShowPaymentError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Payment is invalid or less than the amount due.");
            Console.ResetColor();
        }

        public void ShowPaymentAccepted(double change)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🔔 Payment accepted. Change: {change:N2} PHP");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        public void ShowSearchReservationHeader()
        {
            ClearConsole();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🔍 Search for a Reservation\n");
            Console.ResetColor();
        }

        public void PromptSearchField(int selected)
        {
            Console.Write((selected == 0) ? "Enter Receipt Number: " :
                          (selected == 1) ? "Enter Name: " :
                                            "Enter Contact Number: ");
        }

        public void ShowSearchResults(List<Reservation> results)
        {
            ClearConsole();
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
                    double total = r.FinalTotal;
                    double reservationFee = 500.00;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(divider);
                    Console.WriteLine("        📝 Reservation Receipt            ");
                    Console.WriteLine(divider);
                    Console.ResetColor();
                    Console.WriteLine($"📅 Date        : {date:MMMM dd, yyyy}");
                    Console.WriteLine($"⏰ Time        : {Scheduler.TimeSlots[r.TimeIndex]}");
                    Console.WriteLine($"🍽  Dining Area : {r.DiningArea} - {r.DiningPrice:N2} PHP");
                    Console.WriteLine($"📦 Package     : {r.PackageName} - {r.PackagePrice:N2} PHP");
                    if (r.ExtraItems.Any())
                    {
                        Console.WriteLine("\n🧾 Extras:");
                        var grouped = r.ExtraItems.GroupBy(i => i.Name);
                        foreach (var g in grouped)
                            Console.WriteLine($"• {g.Key} × {g.Count()} - {g.First().Price * g.Count():N2} PHP");
                    }
                    Console.WriteLine(smallDivider);
                    Console.WriteLine($"💰 Subtotal        : {subtotal:N2} PHP");
                    Console.WriteLine($"💸 Discount        : {discount:N2} PHP ({discountDetails})");
                    Console.WriteLine($"💰 Tax (12%)       : {tax:N2} PHP");
                    Console.WriteLine($"💳 Reservation Fee : {reservationFee:N2} PHP");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"💰 Total           : {total:N2} PHP");
                    Console.ResetColor();
                    Console.WriteLine(smallDivider);
                    Console.WriteLine($"👤 Name     : {r.Name}");
                    Console.WriteLine($"📞 Contact  : {r.Contact}");
                    Console.WriteLine($"🪑 Tables   : {r.Tables}");
                    Console.WriteLine($"👥 Guests   : {r.Guests}");
                    Console.WriteLine($"🔖 Reference: {r.ReferenceId}");
                    Console.WriteLine(divider + "\n");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ No reservations found matching your search.");
                Console.ResetColor();
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        public void ShowCancelReservationHeader()
        {
            ClearConsole();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(divider);
            Console.WriteLine("         🔴 Cancel a Reservation         ");
            Console.WriteLine(divider);
            Console.ResetColor();
        }

        public void PromptBookingId()
        {
            Console.Write("🔍 Booking ID: ");
        }

        public void ShowReservationDetails(Reservation reservation, DateTime reservationDate)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + divider);
            Console.WriteLine("         🎫 Reservation Details          ");
            Console.WriteLine(divider);
            Console.ResetColor();
            Console.WriteLine($"🔖 Reference  : {reservation.ReferenceId}");
            Console.WriteLine($"👤 Name       : {reservation.Name}");
            Console.WriteLine($"📞 Contact    : {reservation.Contact}");
            Console.WriteLine($"🪑 Tables     : {reservation.Tables}");
            Console.WriteLine($"👥 Guests     : {reservation.Guests}");
            Console.WriteLine($"📅 Date       : {reservationDate:MMMM dd, yyyy}");
            Console.WriteLine($"⏰ Time       : {Scheduler.TimeSlots[reservation.TimeIndex]}");
            Console.WriteLine($"💰 Total      : {reservation.FinalTotal:N2} PHP");
            Console.WriteLine(divider);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠ Notice: A cancellation fee of 500 PHP will be deducted from your paid amount.");
            Console.ResetColor();
        }

        public void PromptConfirmCancellation()
        {
            Console.Write("\n⚠ Confirm cancellation? (y/n): ");
        }

        public void ShowCancellationComplete(double cancellationFee, double refundAmount)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + divider);
            Console.WriteLine("        ✅ Cancellation Complete         ");
            Console.WriteLine(divider);
            Console.ResetColor();
            Console.WriteLine($"💸 Cancellation Fee : {cancellationFee:N2} PHP");
            Console.WriteLine($"💰 Refund Amount    : {refundAmount:N2} PHP");
            Console.WriteLine(divider);
        }

        public void ShowCancellationAborted()
        {
            Console.WriteLine("\n⚖️ Cancellation aborted.");
        }

        public void ShowNoReservationFound()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n❌ No reservation found with that Booking ID.");
            Console.ResetColor();
        }

        public void ShowPressAnyKey()
        {
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        public void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            ClearConsole();
        }

        public void ShowExitMessage()
        {
            Console.WriteLine("👋 Exiting... Have a great day!");
        }

        public void ShowInvalidInputError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠ Invalid input. Please enter 'y' for yes or 'n' for no.");
            Console.ResetColor();
        }

        private string GetDiscountDetails(double discountAmount, double packagePrice, int guests)
        {
            double perPersonCost = packagePrice / (guests + 1);
            if (Math.Abs(discountAmount - (0.20 * perPersonCost)) < 0.01)
                return "PWD Discount - 20% Off";
            if (Math.Abs(discountAmount - (0.50 * perPersonCost)) < 0.01)
                return "Child Discount - 50% Off";
            if (Math.Abs(discountAmount - perPersonCost) < 0.01)
                return "Infant Discount - 100% Off";
            return "Custom Discount";
        }
    }
}
