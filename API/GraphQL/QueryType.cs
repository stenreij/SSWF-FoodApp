using Core.Domain;
using Core.DomainServices.Interfaces;
using HotChocolate;
using HotChocolate.Types;


namespace API.GraphQL
{
    public class QueryType
    {
        [UsePaging]
        public IQueryable<MealPackage> GetMealPackages([Service] IMealPackageRepo mealPackageRepo) =>
        mealPackageRepo.GetMealPackages().AsQueryable();
        [UsePaging]
        public MealPackage GetMealPackageById(int id, [Service] IMealPackageRepo mealPackageRepo) =>
        mealPackageRepo.GetMealPackageById(id);
    }
}
