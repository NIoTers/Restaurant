using System.Collections.Generic;
using System.Linq;
using System;
using Restaurant;

namespace RestaurantReservation
{
    public class ReservationSystem
    {
        private MenuManager menuManager = new MenuManager();
        private Scheduler availabilityManager = new Scheduler();
        private DisplayManager displayManager = new DisplayManager();
        private ReservationSystemDbAdapter dbAdapter = new ReservationSystemDbAdapter();

        public ReservationSystem()
        {
            int seed;
            using (var db = new ReservationContext())
            {
                var seedInfo = db.SeedInfos.FirstOrDefault();
                if (seedInfo == null)
                {
                    seed = new Random().Next();
                    db.SeedInfos.Add(new SeedInfo { Seed = seed });
                    db.SaveChanges();
                }
                else
                {
                    seed = seedInfo.Seed;
                }
            }
            availabilityManager.InitializeAvailability(DateTime.Today, new DateTime(2026, 6, 30));
            availabilityManager.RandomizeReservations(DateTime.Today, new DateTime(2026, 6, 30), seed);
            var allReservations = dbAdapter.LoadReservations();
            availabilityManager.MarkReservedSlotsFromDb(allReservations);
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                displayManager.ClearConsole();
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
                        displayManager.ShowAboutUs();
                        break;
                    case 5:
                        displayManager.ShowExitMessage();
                        running = false;
                        break;
                }
            }
        }

        private void MakeReservation()
        {
            displayManager.ShowMakeReservationHeader();
            string name;
            while (true)
            {
                displayManager.PromptName();
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) break;
                displayManager.ShowError("Name cannot be blank.");
            }
            string contact;
            while (true)
            {
                displayManager.PromptContact();
                contact = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(contact) && contact.Length >= 7 && contact.All(char.IsDigit)) break;
                displayManager.ShowError("Contact must be at least 7 digits and digits only.");
            }
            int existing = dbAdapter.LoadReservations().Count(r =>
                r.Contact == contact && r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing >= 5)
            {
                displayManager.ShowMaxReservationError();
                return;
            }
            int tables = 0;
            while (true)
            {
                displayManager.PromptTables();
                string input = Console.ReadLine();
                try
                {
                    tables = int.Parse(input);
                    if (tables > 0 && tables <= 3) break;
                }
                catch
                {
                }
                displayManager.ShowError("Invalid number of tables. Please enter a number between 1 and 3.");
            }
            int guests = 0;
            while (true)
            {
                displayManager.PromptGuests();
                string input = Console.ReadLine();
                try
                {
                    guests = int.Parse(input);
                    int minGuests = tables == 1 ? 0 : tables + 1;
                    int maxGuests = tables * 2;
                    if ((tables == 1 && guests == 0) || (guests >= minGuests && guests <= maxGuests)) break;
                }
                catch
                {
                }
                displayManager.ShowGuestRangeError(tables == 1 ? 0 : tables + 1, tables * 2);
            }
            DateTime reservationDate;
            while (true)
            {
                displayManager.PromptDate();
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
                            displayManager.ShowDateError("All time slots are fully booked for this date. Please choose another date.");
                            displayManager.PressAnyKey();
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        displayManager.ShowDateError("The reservation date cannot be in the past. Please select a valid future date.");
                    }
                }
                catch
                {
                    displayManager.ShowDateError("Invalid or out-of-range date. Please try again.");
                }
            }
            int timeIndex = 0;
            while (true)
            {
                displayManager.ClearConsole();
                availabilityManager.ShowAvailability(reservationDate);
                displayManager.PromptTimeSlot();
                string input = Console.ReadLine();
                try
                {
                    timeIndex = int.Parse(input);
                    if (timeIndex >= 1 && timeIndex <= Scheduler.TimeSlots.Length
                        && availabilityManager.IsSlotAvailable(reservationDate, timeIndex - 1))
                    {
                        timeIndex -= 1;
                        break;
                    }
                }
                catch
                {
                }
                displayManager.ShowError("Invalid or unavailable slot. Please choose again.");
                displayManager.PressAnyKey();
            }
            availabilityManager.MarkSlotAsFull(reservationDate, timeIndex);
            var package = menuManager.SelectPackage();
            displayManager.ClearConsole();
            string diningArea;
            if (package.Item1.Contains("VIP"))
            {
                diningArea = "Near Performer";
                displayManager.ShowMessage("Dining area automatically set to Near Performer for VIP packages.");
                displayManager.PressAnyKey();
            }
            else
            {
                diningArea = menuManager.SelectDiningArea(new[] { "Al Fresco", "Dine-In" });
            }
            var extraItems = new Dictionary<MenuItem, int>();
            int extraTotal = 0;
            displayManager.ClearConsole();
            while (true)
            {
                displayManager.PromptAddExtraItem();
                string ans = Console.ReadLine().Trim().ToLower();
                if (ans == "y")
                {
                    var allItems = menuManager.GetAllIndividualItems();
                    displayManager.ShowExtraItems(allItems);
                    string choiceInput = Console.ReadLine();
                    try
                    {
                        int choice = int.Parse(choiceInput);
                        if (choice >= 1 && choice <= allItems.Count)
                        {
                            var item = allItems[choice - 1];
                            displayManager.PromptQuantity();
                            string qtyInput = Console.ReadLine();
                            try
                            {
                                int qty = int.Parse(qtyInput);
                                if (qty > 0)
                                {
                                    if (!extraItems.ContainsKey(item)) extraItems[item] = 0;
                                    extraItems[item] += qty;
                                    extraTotal += item.Price * qty;
                                    displayManager.ShowItemAdded(item.Name, qty);
                                }
                                else
                                {
                                    displayManager.ShowError("Invalid quantity.");
                                }
                            }
                            catch
                            {
                                displayManager.ShowError("Invalid quantity.");
                            }
                        }
                        else
                        {
                            displayManager.ShowError("Invalid selection.");
                        }
                    }
                    catch
                    {
                        displayManager.ShowError("Invalid selection.");
                    }
                    displayManager.PressAnyKey();
                    displayManager.ClearConsole();
                }
                else if (ans == "n")
                {
                    displayManager.HandleNoMoreItems();
                    break;
                }
                else
                {
                    displayManager.ShowInvalidInputError();
                }
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
                DiningArea = diningArea,
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
                while (true)
                {
                    displayManager.PromptDiscount("the PWD discount");
                    string ans = Console.ReadLine().Trim().ToLower();
                    if (ans == "y")
                    {
                        discount = 0.20d * package.Item2;
                        discountDetails = "PWD Discount - 20% Off";
                        break;
                    }
                    else if (ans == "n")
                    {
                        break;
                    }
                    else
                    {
                        displayManager.ShowInvalidInputError();
                    }
                }
            }
            else if (guests > 0)
            {
                while (true)
                {
                    displayManager.PromptDiscount("a discount");
                    string ans = Console.ReadLine().Trim().ToLower();
                    if (ans == "y")
                    {
                        displayManager.ShowDiscountOptions();
                        string discountChoice = Console.ReadLine();
                        switch (discountChoice)
                        {
                            case "1":
                                discount = 0.20d * (package.Item2 / (guests + 1));
                                discountDetails = "PWD Discount - 20% Off";
                                break;
                            case "2":
                                discount = 0.50d * (package.Item2 / (guests + 1));
                                discountDetails = "Child Discount - 50% Off";
                                break;
                            case "3":
                                discount = package.Item2 / (guests + 1);
                                discountDetails = "Infant Discount - 100% Off";
                                break;
                            default:
                                displayManager.ShowError("Invalid discount option.");
                                continue;
                        }
                        break;
                    }
                    else if (ans == "n")
                    {
                        break;
                    }
                    else
                    {
                        displayManager.ShowInvalidInputError();
                    }
                }
            }
            double subtotal = package.Item2 + extraTotal;
            double totalWithDiscount = subtotal - discount;
            double tax = totalWithDiscount * 0.12;
            double reservationFee = 500;
            double finalTotal = totalWithDiscount + tax + reservationFee;
            reservation.DiscountAmount = discount;
            reservation.TaxAmount = tax;
            reservation.FinalTotal = finalTotal;
            while (true)
            {
                displayManager.ClearConsole();
                displayManager.ShowReservationPreview(reservation, reservationDate, package, diningArea);
                string previewConfirm = Console.ReadLine().Trim().ToLower();
                if (previewConfirm == "y")
                {
                    break;
                }
                else if (previewConfirm == "n")
                {
                    displayManager.ShowReservationCanceled();
                    MakeReservation();
                    return;
                }
                else
                {
                    displayManager.ShowInvalidInputError();
                    displayManager.PressAnyKey();
                }
            }
            while (true)
            {
                displayManager.ShowReservationReceipt(reservation, reservationDate, package, diningArea, subtotal, discount, discountDetails, tax, finalTotal, reservationFee);
                string confirm = Console.ReadLine().Trim().ToLower();
                if (confirm == "y")
                {
                    break;
                }
                else if (confirm == "n")
                {
                    availabilityManager.MarkSlotAsAvailable(reservationDate, timeIndex);
                    displayManager.ShowReservationCanceled();
                    MakeReservation();
                    return;
                }
                else
                {
                    displayManager.ShowInvalidInputError();
                    displayManager.PressAnyKey();
                }
            }
            displayManager.ShowReservationReceiptForPayment(reservation, reservationDate, package, diningArea, subtotal, discount, discountDetails, tax, finalTotal, reservationFee);
            dbAdapter.SaveReservation(reservation);
            double payment = 0;
            while (true)
            {
                displayManager.PromptPayment(finalTotal);
                string paymentInput = Console.ReadLine();
                try
                {
                    payment = double.Parse(paymentInput);
                    if (Math.Round(payment, 2) >= Math.Round(finalTotal, 2)) break;
                }
                catch
                {
                }
                displayManager.ShowPaymentError();
            }
            double change = payment - finalTotal;
            displayManager.ShowPaymentAccepted(change);
        }

        private void SearchReservation()
        {
            displayManager.ShowSearchReservationHeader();
            string[] options = { "🔢 Receipt Number", "👤 Name", "📞 Contact", "🔙 Return" };
            int selected = menuManager.SelectMenu("Search Reservations", options);
            if (selected == 3) return;
            displayManager.ClearConsole();
            displayManager.ShowSearchReservationHeader();
            displayManager.PromptSearchField(selected);
            string searchTerm = Console.ReadLine();
            if (selected == 2 && !searchTerm.All(char.IsDigit))
            {
                displayManager.ShowError("Contact number must contain digits only.");
                displayManager.ShowPressAnyKey();
                return;
            }
            var allReservations = dbAdapter.LoadReservations();
            var results = allReservations.Where(r =>
                (selected == 0 && r.ReferenceId.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (selected == 1 && r.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (selected == 2 && r.Contact.Equals(searchTerm))
            ).ToList();
            displayManager.ShowSearchResults(results);
        }

        private void CancelReservation()
        {
            displayManager.ShowCancelReservationHeader();
            displayManager.PromptBookingId();
            string bookingId = Console.ReadLine();
            var foundReservation = dbAdapter.LoadReservations().FirstOrDefault(r => r.ReferenceId == bookingId);
            if (foundReservation != null)
            {
                DateTime reservationDate = new DateTime(foundReservation.Year, foundReservation.MonthIndex + 1, foundReservation.Day);
                if (reservationDate.Date <= DateTime.Today)
                {
                    displayManager.ShowError("You cannot cancel a reservation for today or a past date.");
                    displayManager.ShowPressAnyKey();
                    return;
                }
                displayManager.ShowReservationDetails(foundReservation, reservationDate);
                while (true)
                {
                    displayManager.PromptConfirmCancellation();
                    string confirmCancel = Console.ReadLine().Trim().ToLower();
                    if (confirmCancel == "y")
                    {
                        double cancellationFee = 500.0;
                        double refundAmount = foundReservation.FinalTotal - cancellationFee;
                        dbAdapter.RemoveReservation(bookingId);
                        availabilityManager.MarkSlotAsAvailable(reservationDate, foundReservation.TimeIndex);
                        displayManager.ShowCancellationComplete(cancellationFee, refundAmount);
                        break;
                    }
                    else if (confirmCancel == "n")
                    {
                        displayManager.ShowCancellationAborted();
                        break;
                    }
                    else
                    {
                        displayManager.ShowInvalidInputError();
                    }
                }
            }
            else
            {
                displayManager.ShowNoReservationFound();
            }
            displayManager.ShowPressAnyKey();
        }
    }
}