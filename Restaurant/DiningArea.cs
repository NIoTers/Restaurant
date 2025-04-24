namespace RestaurantReservation
{
    public class DiningArea
    {
        public string Name { get; }
        public int Price { get; }

        public DiningArea(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
