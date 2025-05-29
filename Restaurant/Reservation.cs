using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace RestaurantReservation
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public int Tables { get; set; }
        public int Guests { get; set; }
        public string ReferenceId { get; set; }
        public string PackageName { get; set; }
        public int PackagePrice { get; set; }
        public string DiningArea { get; set; }
        public string ExtraItemsJson { get; set; }
        public int ExtraTotal { get; set; }
        public int MonthIndex { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int TimeIndex { get; set; }
        public double DiscountAmount { get; set; }
        public double TaxAmount { get; set; }
        public double FinalTotal { get; set; }

        [NotMapped]
        public List<MenuItem> ExtraItems
        {
            get => string.IsNullOrEmpty(ExtraItemsJson) ? new List<MenuItem>() : JsonConvert.DeserializeObject<List<MenuItem>>(ExtraItemsJson);
            set => ExtraItemsJson = JsonConvert.SerializeObject(value);
        }

        [NotMapped]
        public int Total => PackagePrice  + ExtraTotal;
    }
}