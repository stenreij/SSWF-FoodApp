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
            return _context.MealPackages.Where(p => p.ReservedByStudent == null).ToList();
        }

        public MealPackage GetMealPackageById(int id)
        {
            return _context.MealPackages.First(p => p.Id == id);
        }

        public IEnumerable<MealPackage> GetReservedMealPackages()
        {
            return _context.MealPackages.Where(p => p.ReservedByStudent != null).ToList();
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
            _context.Update(mealPackage);
            _context.SaveChanges();
        }

    }
}
