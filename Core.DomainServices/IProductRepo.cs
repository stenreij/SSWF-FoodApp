using Core.Domain;
namespace Core.DomainServices
{
    public  interface IProductRepo
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        
    }
}
