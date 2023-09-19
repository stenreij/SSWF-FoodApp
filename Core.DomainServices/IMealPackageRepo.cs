using Core.Domain; 

namespace Core.DomainServices

{
    public  interface IMealPackageRepo
    {
        IEnumerable<MealPackage> GetMealPackages();
        MealPackage GetMealPackageById(int id);
        MealPackage AddMealPackage(MealPackage mealPackage);

    }
}
