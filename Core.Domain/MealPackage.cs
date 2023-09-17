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
        public DateOnly PickUpDate { get; set; }
        public TimeOnly PickUpTime { get; set; }
        public bool AdultsOnly { get; set; }
        public double Price { get; set; }
        public MealType Meal { get; set; }
        public Student? ReservedByStudent { get; set; }
    }
}
