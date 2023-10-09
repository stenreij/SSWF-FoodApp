using Core.Domain;
using Core.DomainServices;

namespace Application.Services
{
    public class MealPackageService : IMealPackageService
    {
        private readonly IMealPackageRepo _mealRepo;

        public MealPackageService(IMealPackageRepo mealPackageRepo)
        {
            _mealRepo = mealPackageRepo;
        }

        public IEnumerable<MealPackage> GetMealPackages()
            => _mealRepo.GetMealPackages();

    }
}
