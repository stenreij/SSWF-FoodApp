﻿using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public  class MealPackageEFRepo : IMealPackageRepo
    {
        private readonly FoodAppDbContext _context;


        public MealPackageEFRepo(FoodAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MealPackage> GetMealPackages()
        {
            return _context.MealPackages
                .Include(m => m.Canteen)
                .Include(m => m.Products)
                .Include(m => m.ReservedByStudent)
                .OrderBy(mp => mp.PickUpDateTime)
                .ToList();
        }

        public IEnumerable<MealPackage> GetAvailableMealPackages()
        {
            return _context.MealPackages
                .Include(m => m.Canteen)
                .Include(m => m.Products)
                .OrderBy(mp => mp.PickUpDateTime)
                .Where(mp => mp.ReservedByStudent == null)
                .ToList();
        }

        public MealPackage GetMealPackageById(int id)
        {
            return _context.MealPackages
                .Include(mp => mp.Canteen)
                .Include(mp => mp.Products)
                .Include (mp => mp.ReservedByStudent)
                .First(mp => mp.Id == id);
        }

        public IEnumerable<MealPackage> GetReservedMealPackages()
        {
            return _context.MealPackages
                .Include(m => m.Canteen)
                .Include(m => m.Products)
                .Include(m => m.ReservedByStudent)
                .OrderBy(mp => mp.PickUpDateTime)
                .Where(mp => mp.ReservedByStudent != null)
                .OrderBy(mp => mp.PickUpDateTime)
                .ToList();
        }

        public IEnumerable<MealPackage> GetReservedMealPackagesByStudent(int studentId)
        {
            return _context.MealPackages
                .Where(mp => mp.ReservedByStudent != null && mp.ReservedByStudent.Id == studentId)
                .OrderBy(mp => mp.PickUpDateTime)
                .ToList();
        }

        public MealPackage AddMealPackage(MealPackage mealPackage)
        {
            _context.MealPackages.Add(mealPackage);
            _context.SaveChanges();
            return mealPackage;
        }

        public void DeleteMealPackage(int id)
        {
            MealPackage mealPackage = _context.MealPackages.Find(id);

            if(mealPackage != null)
            {
                _context.MealPackages.Remove(mealPackage);
                _context.SaveChanges();
            }
        }

        public MealPackage EditMealPackage(MealPackage mealPackage)
        {
            _context.MealPackages.Update(mealPackage);
            _context.SaveChanges();
            return mealPackage;
        }

        public IEnumerable<Product> GetMealPackageProducts(int mealPackageId)
        {
            return _context.MealPackages
                .Where(mp => mp.Id == mealPackageId)
                .SelectMany(mp => mp.Products)
                .ToList();
        }

        public void RemoveProductsFromMealPackage(int mealPackageId, int productId)
        {
            var mealPackage = _context.MealPackages
                .Include(mp => mp.Products)
                .FirstOrDefault(mp => mp.Id == mealPackageId);

            if (mealPackage != null)
            {
                var productToRemove = mealPackage.Products.FirstOrDefault(p => p.Id == productId);

                if (productToRemove != null)
                {
                    mealPackage.Products.Remove(productToRemove);
                    _context.SaveChanges();
                }
            }
        }
        public void AddProductToMealPackage(int mealPackageId, int productId)
        {
            var mealPackage = _context.MealPackages
                .Include(mp => mp.Products)
                .FirstOrDefault(mp => mp.Id == mealPackageId);

            if (mealPackage != null)
            {
                var productToAdd = _context.Products.FirstOrDefault(p => p.Id == productId);

                if (productToAdd != null && !mealPackage.Products.Any(p => p.Id == productId))
                {
                    mealPackage.Products.Add(productToAdd);
                    _context.SaveChanges();
                }
            }
        }

        public MealPackage ReserveMealPackage(int mealPackageId, int studentId)
        {
            var mealPackage = _context.MealPackages
                .Include(mp => mp.Products)
                .Include(mp => mp.ReservedByStudent)
                .FirstOrDefault(mp => mp.Id == mealPackageId);

            if (mealPackage == null)
            {
                throw new NullReferenceException("MealPackage not found");
            }

            if (mealPackage.ReservedByStudent != null)
            {
                throw new Exception("MealPackage already reserved");
            }

            var student = _context.Student.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new NullReferenceException("Student not found");
            }

            mealPackage.ReservedByStudent = student;
            _context.SaveChanges();

            return mealPackage;
        }

        public MealPackage CancelReservation(int mealPackageId, int studentId)
        {
            var mealPackage = _context.MealPackages
                .Include(mp => mp.Products)
                .Include(mp => mp.ReservedByStudent)
                .FirstOrDefault(mp => mp.Id == mealPackageId);

            if (mealPackage == null)
            {
                throw new NullReferenceException("MealPackage not found");
            }

            if (mealPackage.ReservedByStudent == null)
            {
                throw new Exception("You can't cancel this reservation");
            }

            if (mealPackage.ReservedByStudent.Id != studentId)
            {
                throw new Exception("You are not allowed to cancel this reservation");
            }

            mealPackage.ReservedByStudent = null;
            _context.SaveChanges();

            return mealPackage;
        }

        public IEnumerable<MealPackage> GetMealPackagesByCanteenId(int canteenId)
        {
            return _context.MealPackages
                .Include(mp => mp.Canteen)
                .Where(mp => mp.Canteen.Id == canteenId)
                .ToList();
        }

        public void DeleteExpiredMealPackages(DateTime dateTime)
        {
            var expiredMealPackages = _context.MealPackages
                .Where(mp => mp.ExpireDateTime < dateTime)
                .ToList();

            foreach (var mealPackage in expiredMealPackages)
            {
                _context.MealPackages.Remove(mealPackage);
            }
            _context.SaveChanges();
        }
    }
}
