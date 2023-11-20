using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);

    }
}
