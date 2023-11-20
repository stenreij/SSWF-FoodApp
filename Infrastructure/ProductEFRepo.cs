using Core.Domain;
using Core.DomainServices.Interfaces;

namespace Infrastructure
{
    public  class ProductEFRepo : IProductRepo
    {
        private readonly FoodAppDbContext _context;

        public ProductEFRepo(FoodAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.First(p => p.Id == id);
        }
    }
}
