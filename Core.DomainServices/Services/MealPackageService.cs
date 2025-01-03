using Application.Services;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.DomainServices.Services
{
    public class MealPackageService : IMealPackageService
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly ICanteenRepo _canteenRepo;
        private readonly IProductRepo _productRepo;

        public MealPackageService(
            IMealPackageRepo mealPackageRepo, 
            IStudentRepo studentRepo, 
            ICanteenRepo canteenRepo,
            IProductRepo productRepo
            )
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

        public IEnumerable<MealPackage> GetReservedMealPackagesByStudent(int studentId)
             => _mealPackageRepo.GetReservedMealPackagesByStudent(studentId)
            .Where(m => m.ReservedByStudent?.Id == studentId);

        public MealPackage AddMealPackage(MealPackage mealPackage)
        {
            if (mealPackage.PickUpDateTime > mealPackage.ExpireDateTime)
            {
                throw new ArgumentException("Pickup date must be before expiration date.");
            }
            if (mealPackage.PickUpDateTime > DateTime.Now.AddDays(2))
            {
                throw new ArgumentException("Pickup date must be within 2 days.");
            }
            if (mealPackage.Products == null || !mealPackage.Products.Any())
            {
                throw new ArgumentException("At least one product must be added to the meal package.");
            }

            return _mealPackageRepo.AddMealPackage(mealPackage);
        }

        public MealPackage EditMealPackage(MealPackage mealPackage)
        {
            if (mealPackage.PickUpDateTime > mealPackage.ExpireDateTime)
            {
                throw new ArgumentException("Pickup date must be before expiration date.");
            }
            if (mealPackage.PickUpDateTime > DateTime.Now.AddDays(2))
            {
                throw new ArgumentException("Pickup date must be within 2 days.");
            }
            if (mealPackage.Products == null || !mealPackage.Products.Any())
            {
                throw new ArgumentException("At least one product must be added to the meal package.");
            }

            _mealPackageRepo.EditMealPackage(mealPackage);
            return mealPackage;
        }

        public bool DeleteMealPackage(int id)
        {
            var mealPakcage = _mealPackageRepo.GetMealPackageById(id);
            if (mealPakcage.ReservedByStudent != null)
            {
                _mealPackageRepo.DeleteMealPackage(id);
                return false;
            }
            _mealPackageRepo.DeleteMealPackage(id);
            return true;
        }

        public bool ReserveMealPackage(int mealPackageId, int studentId)
        {
            var mealPackage = _mealPackageRepo.GetMealPackageById(mealPackageId);
            var student = _studentRepo.GetStudentById(studentId);

            if (mealPackage.ReservedByStudent != null)
            {
                throw new ArgumentException("This mealpackage is already reserved.");
            }

            if (student.BirthDate.Date > mealPackage.PickUpDateTime.AddYears(-18) && mealPackage.AdultsOnly)
            {
                throw new ArgumentException("You have to be at least 18 to reserve this mealpackage.");
            }

            var reservedMealPackagesForStudent = _mealPackageRepo.GetReservedMealPackagesByStudent(studentId);
            var reservedForSameDay = reservedMealPackagesForStudent
                .Any(mp => mp.PickUpDateTime.Date == mealPackage.PickUpDateTime.Date);

            if (reservedForSameDay == true)
            {
                throw new ArgumentException("You have already reserved a mealpackage for this day.");
            }

            _mealPackageRepo.ReserveMealPackage(mealPackageId, studentId);
            return true;
        }

        public bool CancelReservation(int mealPackageId, int studentId)
        {
            var mealPackage = _mealPackageRepo.GetMealPackageById(mealPackageId);
            var student = _studentRepo.GetStudentById(studentId);

            if (mealPackage.ReservedByStudent != null && mealPackage.ReservedByStudent.Id == student.Id)
            {
                //mealPackage.ReservedByStudent = null;
                _mealPackageRepo.CancelReservation(mealPackageId, studentId);
                return true;
            }
            throw new ArgumentException("You're not allowed to cancel this reservation.");
        }
    }
}
