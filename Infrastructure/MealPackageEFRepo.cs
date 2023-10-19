using Core.Domain;
using Core.DomainServices;
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
            return _context.MealPackages.ToList();
        }

        public IEnumerable<MealPackage> GetAvailableMealPackages()
        {
            var mealPackages = _context.MealPackages
                .Where(p => p.ReservedByStudent == null)
                .ToList();

            mealPackages = mealPackages.OrderBy(p => p.PickUpDateTime).ToList();

            return mealPackages;
        }


        public MealPackage GetMealPackageById(int id)
        {
            return _context.MealPackages
                .First(p => p.Id == id);
        }

        public IEnumerable<MealPackage> GetReservedMealPackages(int studentId)
        {
            return _context.MealPackages
                .Where(p => p.ReservedByStudent != null && p.ReservedByStudent.Id == studentId)
                .OrderBy(p => p.PickUpDateTime)
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

        public void EditMealPackage(MealPackage mealPackage)
        {
            _context.MealPackages.Update(mealPackage);
            _context.SaveChanges();
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
    }
}
