using System;
using System.Collections.Generic;

namespace RestaurantReservation
{
    public static class MenuData
    {
        public static List<MenuItem> AllItems = new List<MenuItem>();
        public static List<Package> Packages = new List<Package>();
        public static List<DiningArea> DiningAreas = new List<DiningArea>();

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


            var solo1 = new Package("Solo Meal Package A", 2100, new List<MenuItem>
            {
                caesarSalad, toguetti, tiramisu
            });
            var solo2 = new Package("Solo Meal Package B", 1950, new List<MenuItem>
            {
                tofuSisig, foe, carrotCake
            });
            var solo3 = new Package("Solo Meal Package C", 1800, new List<MenuItem>
            {
                calamariFritti, soleSpecial, cremaDeLeche
            });

            var couple1 = new Package("Couple Meal Package A", 3950, new List<MenuItem>
            {
                clamChowder, soyChicken, foe, chocolateCake, mashedPotatoes
            });
            var couple2 = new Package("Couple Meal Package B", 3820, new List<MenuItem>
            {
                calamariFritti, braisedBeef, toguetti, tiramisu, coleslaw
            });
            var couple3 = new Package("Couple Meal Package C", 4000, new List<MenuItem>
            {
                tofuSisig, soleSpecial, foe, carrotCake, spinach
            });

            var family1 = new Package("Family Meal Package A", 6350, new List<MenuItem>
            {
                caesarSalad, calamariFritti, braisedBeef, toguetti, tiramisu, chocolateCake, cornCarrots, rice
            });
            var family2 = new Package("Family Meal Package B", 6600, new List<MenuItem>
            {
                clamChowder, tofuSisig, soyChicken, soleSpecial, cremaDeLeche, carrotCake, mashedPotatoes, spinach
            });
            var family3 = new Package("Family Meal Package C", 6200, new List<MenuItem>
            {
                calamariFritti, tofuSisig, braisedBeef, foe, tofuDessert, chocolateCake, coleslaw, rice
            });
            var vip1 = new Package("VIP Meal Package A", 3250, new List<MenuItem>
            {
                clamChowder, braisedBeef, chocolateCake
            });

                        var vip2 = new Package("VIP Meal Package B", 3180, new List<MenuItem>
            {
                calamariFritti, soyChicken, tiramisu
            });

                        var vip3 = new Package("VIP Meal Package C", 3300, new List<MenuItem>
            {
                tofuSisig, soleSpecial, carrotCake
            });
            Packages.AddRange(new[]
            {
                solo1, solo2, solo3,
                couple1, couple2, couple3,
                family1, family2, family3,
                vip1, vip2, vip3
            });
            DiningAreas.AddRange(new[]
            {
                new DiningArea("Al Fresco"),
                new DiningArea("Near Performer"),
                new DiningArea("Dine-In")
            });
        }
    }

    
}
