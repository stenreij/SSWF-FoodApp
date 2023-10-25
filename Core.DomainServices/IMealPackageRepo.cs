﻿using Core.Domain; 

namespace Core.DomainServices

{
    public  interface IMealPackageRepo
    {
        IEnumerable<MealPackage> GetMealPackages();
        IEnumerable<MealPackage> GetAvailableMealPackages();
        IEnumerable<MealPackage> GetReservedMealPackages();
        IEnumerable<MealPackage> GetReservedMealPackagesByStudent(int studentId);
        MealPackage GetMealPackageById(int id);
        MealPackage AddMealPackage(MealPackage mealPackage);
        void EditMealPackage(MealPackage mealPackage);
        void DeleteMealPackage(int id);
        void RemoveProductsFromMealPackage(int mealPackageId, int productId);
        void AddProductToMealPackage(int mealPackageId, int productId);
        void DeleteExpiredMealPackages(DateTime dateTime);
        IEnumerable<Product> GetMealPackageProducts(int mealPackageId);
        MealPackage ReserveMealPackage(int mealPackageId, int studentId);
        IEnumerable<MealPackage> GetMealPackagesByCanteenId(int canteenId);
    }
}
