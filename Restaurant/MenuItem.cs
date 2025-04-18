namespace RestaurantReservation
{
    public class MenuItem
    {
        public string Name { get; }
        public string Category { get; }
        public int Price { get; }

        public MenuItem(string name, string category, int price)
        {
            Name = name;
            Category = category;
            Price = price;
        }

        public override string ToString()
        {
            return Name + " - " + Price + " PHP";
        }
    }
}
