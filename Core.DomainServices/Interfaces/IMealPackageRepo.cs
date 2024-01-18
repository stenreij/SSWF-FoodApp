using Core.Domain;

namespace Core.DomainServices.Interfaces

{
    public interface IMealPackageRepo
    {
        IEnumerable<MealPackage> GetMealPackages();
        IEnumerable<MealPackage> GetAvailableMealPackages();
        IEnumerable<MealPackage> GetReservedMealPackages();
        IEnumerable<MealPackage> GetReservedMealPackagesByStudent(int studentId);
        MealPackage GetMealPackageById(int id);
        IEnumerable<MealPackage> GetMealPackagesByCanteenId(int id);
        MealPackage AddMealPackage(MealPackage mealPackage);
        MealPackage EditMealPackage(MealPackage mealPackage);
        void DeleteMealPackage(int id);
        void RemoveProductsFromMealPackage(int mealPackageId, int productId);
        void AddProductToMealPackage(int mealPackageId, int productId);
        void DeleteExpiredMealPackages(DateTime dateTime);
        IEnumerable<Product> GetMealPackageProducts(int mealPackageId);
        MealPackage? ReserveMealPackage(int mealPackageId, int studentId);
        MealPackage? CancelReservation(int mealPackageId, int studentId);
        object ExecuteQuery(string query);
    }
}
