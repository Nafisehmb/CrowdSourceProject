using System;
namespace DuckFeeding.Models
{
    public class DuckFeedingRecord
    {
        public long Id { get; set; }
        public string Time { get; set; }
        public string Food { get; set; }
        public string Location { get; set; }
        public string FoodType { get; set; }
        public string Count { get; set; }
        public string FoodAmount { get; set; }
        public bool Repeat { get; set; }
        public string Period { get; set; }
    }
}
