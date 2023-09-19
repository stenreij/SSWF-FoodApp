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

        public MealPackage GetMealPackageById(int id)
        {
            return _context.MealPackages.First(p => p.Id == id);
        }

        public MealPackage AddMealPackage(MealPackage mealPackage)
        {
            _context.MealPackages.Add(mealPackage);
            _context.SaveChanges();
            return mealPackage;
        }
    }
}
