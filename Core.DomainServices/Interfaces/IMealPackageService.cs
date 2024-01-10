using Core.Domain;
using Core.DomainServices;

namespace Application.Services
{
    public interface IMealPackageService
    {
        IEnumerable<MealPackage> GetMealPackages();
        IEnumerable<MealPackage> GetAvailableMealPackages();
        IEnumerable<MealPackage> GetReservedMealPackages();
        bool ReserveMealPackage(int mealPackageId, int studentId);
    }
}