using System.Collections.Generic;

namespace RestaurantReservation
{
    public class Package
    {
        public string Name { get; }
        public int TotalPrice { get; }
        public List<MenuItem> Items { get; }

        public Package(string name, int totalPrice, List<MenuItem> items)
        {
            Name = name;
            TotalPrice = totalPrice;
            Items = items;
        }
    }
}
