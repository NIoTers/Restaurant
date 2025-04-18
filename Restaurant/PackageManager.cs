using System;

namespace RestaurantReservation
{
    public class PackageManager
    {
        private string[] packages = {
            "Package A - 15,000 PHP",
            "Package B - 18,000 PHP",
            "Package C - 16,000 PHP",
            "Back to Main Menu"
        };

        public void ShowPackages()
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Select a Package - Use Arrow Keys to Navigate, Enter to Select\n");

                for (int i = 0; i < packages.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(">> " + packages[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("   " + packages[i]);
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                    selectedIndex = (selectedIndex == 0) ? packages.Length - 1 : selectedIndex - 1;
                else if (key == ConsoleKey.DownArrow)
                    selectedIndex = (selectedIndex == packages.Length - 1) ? 0 : selectedIndex + 1;

            } while (key != ConsoleKey.Enter);

            if (selectedIndex == packages.Length - 1)
            {
                return;
            }

            ShowPackageDetails(selectedIndex);
        }

        private void ShowPackageDetails(int packageIndex)
        {
            Console.Clear();
            string packageDetails = GetPackageDetails(packageIndex);

            Console.WriteLine(packageDetails);
            Console.WriteLine("\nPress any key to go back...");
            Console.ReadKey();
            ShowPackages();
        }

        private string GetPackageDetails(int index)
        {
            string packageDetails = "";

            switch (index)
            {
                case 0:
                    packageDetails = "Package A - 15,000 PHP\n\n" +
                                     "• Caesar Salad\n" +
                                     "• Toguetti\n" +
                                     "• FOE (Fried on Everything)\n" +
                                     "• Silken Tofu w/ Tapioca Pearl & Caramel\n" +
                                     "• Carrot Cake\n" +
                                     "• Mashed Potatoes";
                    break;

                case 1:
                    packageDetails = "Package B - 18,000 PHP\n\n" +
                                     "• Clam Chowder\n" +
                                     "• Tofu Sisig\n" +
                                     "• Braised Beef w/ Silken Tofu\n" +
                                     "• Sole Special\n" +
                                     "• Crema de Leche\n" +
                                     "• Tiramisu\n" +
                                     "• Corn & Carrots\n" +
                                     "• Coleslaw";
                    break;

                case 2:
                    packageDetails = "Package C - 16,000 PHP\n\n" +
                                     "• Calamari Fritti\n" +
                                     "• Slow Cooked Chicken Marinated in Soy Sauce\n" +
                                     "• FOE (Fried on Everything)\n" +
                                     "• Moist Chocolate Cake\n" +
                                     "• Carrot Cake\n" +
                                     "• Creamy Spinach\n" +
                                     "• Rice";
                    break;

                default:
                    packageDetails = "Invalid Selection";
                    break;
            }

            return packageDetails;
        }

    }
}
