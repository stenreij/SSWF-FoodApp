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
