using Core.Domain; 

namespace Core.DomainServices

{
    public  interface IMealPackageRepo
    {
        IEnumerable<MealPackage> GetMealPackages();
    }
}
