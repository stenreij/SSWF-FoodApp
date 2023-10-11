using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FoodApp.Models;

public class MealPackageViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Product>? Products { get; set; }
    public City City { get; set; }
    public int CanteenId { get; set; }
    public DateTime PickUpDateTime { get; set; }
    public DateTime ExpireDateTime { get; set; }
    public bool AdultsOnly { get; set; }
    public double Price { get; set; }
    public MealType MealType { get; set; }
    public Student? ReservedByStudent { get; set; }
    public List<int>? SelectedProducts { get; set; }
    public List<ProductCheckBoxes> ProductCheckBoxes { get; set; } = new List<ProductCheckBoxes>();

}

