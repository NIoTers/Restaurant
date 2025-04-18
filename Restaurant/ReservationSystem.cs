using System;
using System.Collections.Generic;

namespace RestaurantReservation
{
    public class ReservationSystem
    {
        private readonly MenuManager menuManager = new MenuManager();
        private readonly AvailabilityManager availabilityManager = new AvailabilityManager();

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
                        availabilityManager.ShowSchedule();
                        break;
                    case 3:
                        AboutUs();
                        break;
                    case 4:
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
            Console.WriteLine("👨‍💻 Charles Andrei S. Alarcon    - Code Optimization");
            Console.WriteLine("🎨 Princess Naoebe P. Dizon     - UI Design & Documentation");
            Console.WriteLine("📆 Joseph Andrew C. Fernandez   - Scheduling Module");
            Console.WriteLine("📦 Adrian Kyle C. Garfin        - View Packages Feature");
            Console.WriteLine("📝 Gwyneth B. Saga              - Make Reservation Feature");

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }



        private void MakeReservation()
        {
            Console.Clear();
            Console.WriteLine("📝 Make a Reservation\n");

            string name = "";
            while (true)
            {
                Console.Write("👤 Name: ");
                name = Console.ReadLine();
                try
                {
                    if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name cannot be blank.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ " + ex.Message);
                    Console.ResetColor();
                }
            }

            string contact = "";
            while (true)
            {
                Console.Write("📞 Contact Number: ");
                contact = Console.ReadLine();
                try
                {
                    if (string.IsNullOrWhiteSpace(contact)) throw new Exception("Contact number cannot be blank.");
                    foreach (char c in contact)
                    {
                        if (!char.IsDigit(c)) throw new Exception("Contact must be digits only.");
                    }
                    if (contact.Length < 7) throw new Exception("Contact must be at least 7 digits.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ " + ex.Message);
                    Console.ResetColor();
                }
            }

            int guests = 0;
            while (true)
            {
                Console.Write("👥 Number of Guests: ");
                string input = Console.ReadLine();
                try
                {
                    guests = int.Parse(input);
                    if (guests <= 0) throw new Exception("Must be at least 1 guest.");
                    break;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ Invalid number of guests.");
                    Console.ResetColor();
                }
            }

            int monthIndex = availabilityManager.SelectMonth();
            availabilityManager.InitializeMonth(monthIndex);
            int day = availabilityManager.SelectDate(monthIndex);
            int timeIndex = availabilityManager.SelectTimeSlot(monthIndex, day, false);

            Tuple<string, int> package = menuManager.SelectPackage();
            string packageName = package.Item1;
            int packagePrice = package.Item2;

            Tuple<string, int> venue = menuManager.SelectDiningArea();
            string diningArea = venue.Item1;
            int diningPrice = venue.Item2;

            Dictionary<MenuItem, int> extraItems = new Dictionary<MenuItem, int>();
            int extraTotal = 0;

            while (true)
            {
                Console.Write("➕ Would you like to add an extra item? (y/n): ");
                string answer = Console.ReadLine().Trim().ToLower();
                if (answer != "y" && answer != "n")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ Please enter 'y' or 'n'.");
                    Console.ResetColor();
                    continue;
                }
                if (answer == "n") break;

                List<MenuItem> allItems = menuManager.GetAllIndividualItems();

                Console.Clear();
                Console.WriteLine("📋 Available Items:\n");
                for (int i = 0; i < allItems.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + allItems[i]);
                }

                Console.Write("\nEnter the item number to add: ");
                string itemInput = Console.ReadLine();
                int choice;
                if (!int.TryParse(itemInput, out choice) || choice < 1 || choice > allItems.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ Invalid selection.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                MenuItem selected = allItems[choice - 1];

                Console.Write("🧮 Enter quantity: ");
                string qtyInput = Console.ReadLine();
                int quantity;
                if (!int.TryParse(qtyInput, out quantity) || quantity <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠ Invalid quantity.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (extraItems.ContainsKey(selected))
                    extraItems[selected] += quantity;
                else
                    extraItems[selected] = quantity;

                int itemSubtotal = selected.Price * quantity;
                extraTotal += itemSubtotal;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✔ " + selected.Name + " × " + quantity + " added (" + itemSubtotal + " PHP)");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            int total = packagePrice + diningPrice + extraTotal;
            string reference = "RES-" + new Random().Next(1000, 9999).ToString();
            availabilityManager.MarkSlotAsFull(monthIndex, day, timeIndex);

            List<MenuItem> flatExtras = new List<MenuItem>();
            foreach (var entry in extraItems)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    flatExtras.Add(entry.Key);
                }
            }

            string resKey = monthIndex + "-" + day + "-" + timeIndex;
            Reservation reservation = new Reservation
            {
                Name = name,
                Contact = contact,
                Guests = guests,
                ReferenceId = reference,
                PackageName = packageName,
                PackagePrice = packagePrice,
                DiningArea = diningArea,
                DiningPrice = diningPrice,
                ExtraItems = flatExtras,
                ExtraTotal = extraTotal,
                MonthIndex = monthIndex,
                Day = day,
                TimeIndex = timeIndex
            };

            availabilityManager.BookedReservations[resKey] = reservation;

            Console.Clear();
            Console.WriteLine("✅ Reservation Confirmed!");
            Console.WriteLine("📅 Date: " + AvailabilityManager.Months[monthIndex] + " " + day);
            Console.WriteLine("⏰ Time: " + AvailabilityManager.TimeSlots[timeIndex]);
            Console.WriteLine("🍽 Venue: " + diningArea + " - " + diningPrice + " PHP");
            Console.WriteLine("📦 Package: " + packageName + " - " + packagePrice + " PHP");

            if (extraItems.Count > 0)
            {
                Console.WriteLine("\n🧾 Additional Items:");
                foreach (var entry in extraItems)
                {
                    string itemLine = "• " + entry.Key.Name + " × " + entry.Value;
                    int subtotal = entry.Key.Price * entry.Value;
                    itemLine += " = " + subtotal + " PHP";
                    Console.WriteLine(itemLine);
                }
                Console.WriteLine("➕ Extra Items Total: " + extraTotal + " PHP");
            }

            Console.WriteLine("\n💰 Total: " + total + " PHP");
            Console.WriteLine("👤 Name: " + name);
            Console.WriteLine("📞 Contact: " + contact);
            Console.WriteLine("👥 Guests: " + guests);
            Console.WriteLine("🔖 Reference ID: " + reference);
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }


    }
}
