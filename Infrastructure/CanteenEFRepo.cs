using Core.Domain;
using Core.DomainServices.Interfaces;

namespace Infrastructure
{
    public class CanteenEFRepo : ICanteenRepo
    {
        private readonly FoodAppDbContext _context;

        public CanteenEFRepo(FoodAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Canteen> GetCanteens()
        {
            return _context.Canteens.ToList();
        }

        public Canteen GetCanteenById(int id)
        {
            return _context.Canteens.First(p => p.Id == id);
        }

    }
}
