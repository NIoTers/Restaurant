using System.Collections.Generic;

namespace RestaurantReservation
{
    public static class MenuData
    {
        public static List<MenuItem> AllItems = new List<MenuItem>();
        public static List<Package> Packages = new List<Package>();

        static MenuData()
        {
            var caesarSalad = new MenuItem("Caesar Salad", "Appetizer", 480);
            var calamariFritti = new MenuItem("Calamari Fritti", "Appetizer", 520);
            var clamChowder = new MenuItem("Clam Chowder", "Appetizer", 550);
            var tofuSisig = new MenuItem("Tofu Sisig", "Appetizer", 500);

            var braisedBeef = new MenuItem("Braised Beef w/ Silken Tofu", "Main Course", 1600);
            var foe = new MenuItem("FOE (Fried On Everything)", "Main Course", 1300);
            var toguetti = new MenuItem("Toguetti", "Main Course", 1400);
            var soyChicken = new MenuItem("Slow Cooked Chicken Marinated in Soy Sauce", "Main Course", 1450);
            var soleSpecial = new MenuItem("Sole Special", "Main Course", 1350);

            var tofuDessert = new MenuItem("Silken Tofu w/ Tapioca Pearls & Caramel", "Dessert", 650);
            var cremaDeLeche = new MenuItem("Crema de Leche", "Dessert", 600);
            var tiramisu = new MenuItem("Tiramisu", "Dessert", 700);
            var chocolateCake = new MenuItem("Moist Chocolate Cake", "Dessert", 750);
            var carrotCake = new MenuItem("Carrot Cake", "Dessert", 680);

            var mashedPotatoes = new MenuItem("Mashed Potatoes", "Side Dish", 550);
            var cornCarrots = new MenuItem("Corn & Carrots", "Side Dish", 500);
            var coleslaw = new MenuItem("Coleslaw", "Side Dish", 500);
            var spinach = new MenuItem("Creamy Spinach", "Side Dish", 520);
            var rice = new MenuItem("Shiritaki Rice", "Side Dish", 250);

            AllItems.AddRange(new[]
            {
                caesarSalad, calamariFritti, clamChowder, tofuSisig,
                braisedBeef, foe, toguetti, soyChicken, soleSpecial,
                tofuDessert, cremaDeLeche, tiramisu, chocolateCake, carrotCake,
                mashedPotatoes, cornCarrots, coleslaw, spinach, rice
            });

            var packageA = new Package("Package A", 5060, new List<MenuItem>
            {
                caesarSalad,
                toguetti,
                foe,
                tofuDessert,
                carrotCake,
                mashedPotatoes,
            });

            var packageB = new Package("Package B", 6300, new List<MenuItem>
            {
                clamChowder,
                tofuSisig,
                braisedBeef,
                soleSpecial,
                cremaDeLeche,
                tiramisu,
                cornCarrots,
                coleslaw,
            });

            var packageC = new Package("Package C", 5320, new List<MenuItem>
            {
                calamariFritti,
                soyChicken,
                foe,
                chocolateCake,
                carrotCake,
                spinach,
                rice,
            });

            Packages.Add(packageA);
            Packages.Add(packageB);
            Packages.Add(packageC);
        }
    }
}
