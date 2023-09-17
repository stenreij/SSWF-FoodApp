using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Canteen
    {
        public int Id { get; set; }
        public City City { get; set; }
        public Location Location { get; set; }
        public bool? WarmMeal { get; set; }
        public string? CanteenName { get; set; }
    }
}
