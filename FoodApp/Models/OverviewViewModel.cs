using Core.Domain;

namespace FoodApp.Models
{
    public class OverviewViewModel
    {
        public List<MealPackage> CanteenMealPackages { get; set; }
        public List<MealPackage> OtherCanteenMealPackages { get; set; }
        public List<MealPackage> StudentMealPackages { get; set; }
        public string? CanteenName { get; internal set; }
    }
}
