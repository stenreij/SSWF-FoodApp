using Core.Domain; 

namespace Core.DomainServices

{
    public  interface IMealPackageRepo
    {
        IEnumerable<MealPackage> GetMealPackages();
        IEnumerable<MealPackage> GetAvailableMealPackages();
        IEnumerable<MealPackage> GetReservedMealPackages();
        MealPackage GetMealPackageById(int id);
        MealPackage AddMealPackage(MealPackage mealPackage);
        void EditMealPackage(MealPackage mealPackage);
        void DeleteMealPackage(int id);
    }
}
