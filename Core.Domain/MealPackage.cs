using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{

    public class MealPackage
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "")]
        public string Name { get; set; } = null!;
        public ICollection<Product>? Products { get; set; }
        public City City { get; set; }
        public Canteen? Canteen { get; set; }
        public int CanteenId { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public bool AdultsOnly { get; set; }
        public double Price { get; set; }
        public MealType MealType { get; set; }
        public Student? ReservedByStudent { get; set; }
    }
}
