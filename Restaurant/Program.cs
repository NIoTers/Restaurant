using System;

namespace Restaurant
{
    internal class Program
    {
        static string selectedPackage;
        static int packagePrice;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ShowMainMenu();
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("✨ Welcome to M.A.R.I.L.A.G Restaurant Reservation System ✨\n");
                Console.WriteLine("1. 🍽 View Packages");
                Console.WriteLine("2. 📅 Make a Reservation (not implemented yet)");
                Console.WriteLine("3. 📆 View Schedule (not implemented yet)");
                Console.WriteLine("4. ❌ Exit");
                Console.Write("\nSelect an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ViewPackages();
                        break;
                    case "2":
                        StartReservation();
                        break;
                    case "3":
                        ShowSchedule();
                        break;
                    case "4":
                        Console.WriteLine("\n👋 Exiting... Have a great day!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ViewPackages()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select a Package to View\n");
                Console.WriteLine("1. Package A - 15,000 PHP");
                Console.WriteLine("2. Package B - 18,000 PHP");
                Console.WriteLine("3. Package C - 16,000 PHP");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("\nSelect an option: ");
                string input = Console.ReadLine();

                if (input == "4")
                {
                    break;
                }
                else if (input == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Package A - 15,000 PHP\n");
                    Console.WriteLine("• Caesar Salad");
                    Console.WriteLine("• Toguetti");
                    Console.WriteLine("• FOE (Fried on Everything)");
                    Console.WriteLine("• Silken Tofu w/ Tapioca Pearl & Caramel");
                    Console.WriteLine("• Carrot Cake");
                    Console.WriteLine("• Mashed Potatoes");
                }
                else if (input == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Package B - 18,000 PHP\n");
                    Console.WriteLine("• Clam Chowder");
                    Console.WriteLine("• Tofu Sisig");
                    Console.WriteLine("• Braised Beef w/ Silken Tofu");
                    Console.WriteLine("• Sole Special");
                    Console.WriteLine("• Crema de Leche");
                    Console.WriteLine("• Tiramisu");
                    Console.WriteLine("• Corn & Carrots");
                    Console.WriteLine("• Coleslaw");
                }
                else if (input == "3")
                {
                    Console.Clear();
                    Console.WriteLine("Package C - 16,000 PHP\n");
                    Console.WriteLine("• Calamari Fritti");
                    Console.WriteLine("• Slow Cooked Chicken Marinated in Soy Sauce");
                    Console.WriteLine("• FOE (Fried on Everything)");
                    Console.WriteLine("• Moist Chocolate Cake");
                    Console.WriteLine("• Carrot Cake");
                    Console.WriteLine("• Creamy Spinach");
                    Console.WriteLine("• Rice");
                }
                else
                {
                    Console.WriteLine("\nInvalid option.");
                }
                Console.WriteLine("\nPress any key to return to the package list...");
                Console.ReadKey();
            }
        }

        static void StartReservation()
        {
            Console.Clear();
            Console.WriteLine("📅 Make a Reservation is not yet finish.");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        static void ShowSchedule()
        {
            Console.Clear();
            Console.WriteLine("📆 View Schedule is not yet finish.");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
