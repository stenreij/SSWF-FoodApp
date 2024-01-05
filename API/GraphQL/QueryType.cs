using Core.Domain;
using Core.DomainServices.Interfaces;
using HotChocolate;
using HotChocolate.Types;


namespace API.GraphQL
{
    public class QueryType
    {
        [UsePaging]
        public IEnumerable<MealPackage> GetMealPackages([Service] IMealPackageRepo mealPackageRepo, [Service] ICanteenRepo canteenRepo) =>
        mealPackageRepo.GetMealPackages().AsQueryable();
 
        public MealPackage GetMealPackageById(int id, [Service] IMealPackageRepo mealPackageRepo, [Service] ICanteenRepo canteenRepo) =>
        mealPackageRepo.GetMealPackageById(id);
    }
}
