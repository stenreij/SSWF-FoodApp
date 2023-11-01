using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{

    public class MealPackage
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } =  string.Empty!;
        public City City { get; set; }
        [Required(ErrorMessage = "Canteen is required.")]
        public Canteen Canteen { get; set; } = null!;
        [Required(ErrorMessage = "Pick up date/time is required.")]
        public DateTime PickUpDateTime { get; set; }
        [Required(ErrorMessage = "Expire date/time is required.")]
        public DateTime ExpireDateTime { get; set; }
        public bool AdultsOnly { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 800, ErrorMessage = "Price must be between 0.01 and 100.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Meal type is required.")]
        public MealType MealType { get; set; }
        public Student? ReservedByStudent { get; set; }
        public ICollection<Product> Products { get; set; } = null!;
    }
}
