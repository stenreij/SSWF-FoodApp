using Core.Domain;
using Core.DomainServices;

namespace Application.Services
{
    public class MealPackageService : IMealPackageService
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly ICanteenRepo _canteenRepo;

        public MealPackageService(IMealPackageRepo mealPackageRepo, IStudentRepo studentRepo, ICanteenRepo canteenRepo)
        {
            _mealPackageRepo = mealPackageRepo;
            _studentRepo = studentRepo;
            _canteenRepo = canteenRepo;
        }

        public IEnumerable<MealPackage> GetAvailableMealPackages()
            => _mealPackageRepo.GetAvailableMealPackages()
            .Where(m => m.ReservedByStudent == null);

        public IEnumerable<MealPackage> GetReservedMealPackages()
            => _mealPackageRepo.GetReservedMealPackages()
            .Where(m => m.ReservedByStudent != null);

        public IEnumerable<MealPackage> GetMealPackages()
            => _mealPackageRepo.GetMealPackages();

        public MealPackage GetMealPackageById(int id)
            => _mealPackageRepo.GetMealPackageById(id);

        public IEnumerable<MealPackage> GetMealPackagesByCanteenId(int id)
            => _mealPackageRepo.GetMealPackagesByCanteenId(id)
            .Where(m => m.Canteen.Id == id);

        public IEnumerable<MealPackage> GetMealPackagesOtherCanteens(int id)
            => _mealPackageRepo.GetMealPackages()
            .Where(m => m.Canteen.Id != id);

        public bool ReserveMealPackage(int mealPackageId, int studentId)
        {
            var mealPackage = _mealPackageRepo.GetMealPackageById(mealPackageId);

            if (mealPackage.ReservedByStudent != null)
            {
                return false;
            }

            _mealPackageRepo.ReserveMealPackage(mealPackageId, studentId);
            return true;
        }
    }
}
