using Core.Domain;
using Core.DomainServices;

namespace ApplicationService
{

    public class MealPackageService : IMealPackageService
    {
        private readonly IMealPackageRepo _mealRepo;

        public MealPackageService(IMealPackageRepo mealPackageRepo) 
        {
        _mealRepo = mealPackageRepo;
        }

        //Get MealPackages ordered by pick up date/time (and not reserved yet?)
        public IEnumerable<MealPackage> GetMealPackages() 
            => _mealRepo.GetMealPackages()
            ;

    }

}