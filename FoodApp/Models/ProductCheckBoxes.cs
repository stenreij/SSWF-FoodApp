using Core.Domain;

namespace FoodApp.Models
{
    public class ProductCheckBoxes
    {
        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
        public bool IsSelected { get; set; }
    }
}
