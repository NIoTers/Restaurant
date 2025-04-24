using System.Collections.Generic;

namespace RestaurantReservation
{
    public class Reservation
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public int Tables { get; set; }
        public int Guests { get; set; }
        public string ReferenceId { get; set; }
        public string PackageName { get; set; }
        public int PackagePrice { get; set; }
        public string DiningArea { get; set; }
        public int DiningPrice { get; set; }
        public List<MenuItem> ExtraItems { get; set; }
        public int ExtraTotal { get; set; }
        public int MonthIndex { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }

        public int TimeIndex { get; set; }
        public int Total => PackagePrice + DiningPrice + ExtraTotal;

        public double DiscountAmount { get; set; }
        public double TaxAmount { get; set; }
        public double FinalTotal { get; set; }
    }
}
