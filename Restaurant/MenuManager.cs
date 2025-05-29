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
                "📆 Search Schedule",
                "❌ Cancel Reservation",
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
                string[] categories = { "👤 Solo Meal Packages", "💑 Couple Meal Packages", "👨👩👧👦 Family Meal Packages", "🌟 VIP Meal Packages", "🔙 Return" };
                int selectedCategory = SelectMenu("🍱 Choose a Package Category", categories);
                if (selectedCategory == 4) return;
                string keyword = selectedCategory == 0 ? "Solo" :
                                selectedCategory == 1 ? "Couple" :
                                selectedCategory == 2 ? "Family" :
                                "VIP";
                var filteredPackages = MenuData.Packages
                    .Where(p => p.Name.Contains(keyword))
                    .ToList();
                if (filteredPackages.Count == 0)
                {
                    Console.WriteLine("No packages found in this category.");
                    Console.ReadKey();
                    continue;
                }
                string emoji = selectedCategory == 0 ? "👤" :
                              selectedCategory == 1 ? "💑" :
                              selectedCategory == 2 ? "👨👩👧👦" :
                              "🌟";
                while (true)
                {
                    var packageNamesWithReturn = filteredPackages
                        .Select(p => emoji + " " + p.Name + " - " + p.TotalPrice + " PHP")
                        .ToList();
                    packageNamesWithReturn.Add("🔙 Return");
                    int selectedPackage = SelectMenu($"📦 {categories[selectedCategory]}", packageNamesWithReturn.ToArray());
                    if (selectedPackage == packageNamesWithReturn.Count - 1) break;
                    Console.Clear();
                    var package = filteredPackages[selectedPackage];
                    Console.WriteLine($"📦 {emoji} {package.Name} - {package.TotalPrice} PHP\n");
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
                    }
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the package list...");
                    Console.ReadKey();
                }
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
            while (true)
            {
                var packageOptions = MenuData.Packages
                    .Select(p => $"{p.Name} - {p.TotalPrice} PHP")
                    .ToArray();
                int selectedIndex = SelectMenu("📦 Choose Your Package", packageOptions);
                var selectedPackage = MenuData.Packages[selectedIndex];

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"📦 Package Details: {selectedPackage.Name}");
                Console.WriteLine("═══════════════════════════════════════════");
                Console.ResetColor();
                Console.WriteLine($"Price: {selectedPackage.TotalPrice:N2} PHP");
                Console.WriteLine("Contents:");
                var grouped = selectedPackage.Items
                    .GroupBy(item => item.Category)
                    .OrderBy(g => g.Key);
                foreach (var group in grouped)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{group.Key}]");
                    Console.ResetColor();
                    foreach (var item in group)
                    {
                        Console.WriteLine($"• {item.Name} - {item.Price} PHP");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("───────────────────────────────────────────");
                while (true)
                {
                    Console.Write("❓ Do you want this package? (y/n): ");
                    string confirm = Console.ReadLine().Trim().ToLower();
                    if (confirm == "y")
                    {
                        return new Tuple<string, int>(selectedPackage.Name, selectedPackage.TotalPrice);
                    }
                    else if (confirm == "n")
                    {
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("⚠ Invalid input. Please enter 'y' for yes or 'n' for no.");
                        Console.ResetColor();
                    }
                }
            }
        }

        public string SelectDiningArea(string[] allowedAreas)
        {
            int selected = SelectMenu("🍽 Choose your dining area", allowedAreas);
            return allowedAreas[selected];
        }

        public List<MenuItem> GetAllIndividualItems()
        {
            return MenuData.AllItems;
        }

        public int SelectMenu(string title, string[] options)
        {
            int index = 0;
            Console.CursorVisible = false;
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
                var key = Console.ReadKey(true).Key;
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
