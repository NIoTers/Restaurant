using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantReservation
{
    public class MenuManager
    {
        public int ShowMainMenu()
        {
            string[] menu = {
                "📖 View Menu",
                "📅 Make a Reservation",
                "📆 View Schedule",
                "ℹ️ About Us",
                "❌ Exit"
            };
            return SelectMenu("✨ Welcome to the M.A.R.I.L.A.G. Restaurant Reservation System ✨", menu);
        }

        public void ViewMenu()
        {
            while (true)
            {
                string[] options = { "📦 View Packages", "🍴 View Individual Items", "🔙 Return" };
                int selected = SelectMenu("📖 View Menu\n📋 Select a Menu Option", options);

                if (selected == 2) return;

                if (selected == 0)
                    ViewPackages();
                else if (selected == 1)
                    ViewIndividualItems();
            }
        }

        public void ViewPackages()
        {
            while (true)
            {
                string[] menu = { "Package A", "Package B", "Package C", "🔙 Return" };
                int selected = SelectMenu("🍱 Select a Package to View", menu);
                if (selected == 3) return;

                Console.Clear();
                var package = MenuData.Packages[selected];
                Console.WriteLine($"📦 {package.Name} - {package.TotalPrice} PHP\n");

                var grouped = new Dictionary<string, List<MenuItem>>();

                foreach (var item in package.Items)
                {
                    if (!grouped.ContainsKey(item.Category))
                        grouped[item.Category] = new List<MenuItem>();

                    grouped[item.Category].Add(item);
                }

                foreach (var category in grouped.Keys)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{category}]");
                    Console.ResetColor();

                    foreach (var item in grouped[category])
                    {
                        Console.WriteLine($"• {item.Name} - {item.Price} PHP");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
            }
        }

        public void ViewIndividualItems()
        {
            Console.Clear();
            Console.WriteLine("📋 Individual Menu Items with Prices\n");

            var grouped = MenuData.AllItems
                .GroupBy(item => item.Category)
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[" + group.Key + "]");
                Console.ResetColor();

                foreach (var item in group)
                {
                    Console.WriteLine("• " + item.Name + " - " + item.Price + " PHP");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        public Tuple<string, int> SelectPackage()
        {
            string[] options = MenuData.Packages
                .Select(p => p.Name + " - " + p.TotalPrice + " PHP").ToArray();

            int selected = SelectMenu("📦 Choose your package", options);
            var package = MenuData.Packages[selected];
            return new Tuple<string, int>(package.Name, package.TotalPrice);
        }

        public Tuple<string, int> SelectDiningArea()
        {
            string[] venues = { "Al Fresco - 20,000 PHP", "Near Performer - 22,000 PHP", "Dine-In - 18,000 PHP" };
            int[] prices = { 20000, 22000, 18000 };

            int selected = SelectMenu("🍽 Choose your dining area", venues);
            string venueName = venues[selected].Split('-')[0].Trim();
            return new Tuple<string, int>(venueName, prices[selected]);
        }

        public List<MenuItem> GetAllIndividualItems()
        {
            return MenuData.AllItems;
        }

        private int SelectMenu(string title, string[] options)
        {
            int index = 0;
            Console.CursorVisible = false;

            int startRow = Console.CursorTop;
            Console.Clear();
            Console.WriteLine(title + "\n");

            int titleRows = Console.CursorTop;

            for (int i = 0; i < options.Length; i++)
            {
                Console.ForegroundColor = (i == index) ? ConsoleColor.Green : ConsoleColor.Gray;
                Console.WriteLine((i == index ? ">> " : "   ") + options[i]);
            }

            Console.ResetColor();

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                int oldIndex = index;

                if (key == ConsoleKey.UpArrow) index = (index == 0) ? options.Length - 1 : index - 1;
                else if (key == ConsoleKey.DownArrow) index = (index == options.Length - 1) ? 0 : index + 1;
                else if (key == ConsoleKey.Enter) return index;

                if (index != oldIndex)
                {
                    Console.SetCursorPosition(0, titleRows + oldIndex);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("   " + options[oldIndex]);

                    Console.SetCursorPosition(0, titleRows + index);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(">> " + options[index]);

                    Console.ResetColor();
                }
            }
        }
    }
}
