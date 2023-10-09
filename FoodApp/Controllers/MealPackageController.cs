using Application.Services;
using Core.Domain;
using Core.DomainServices;
using FoodApp.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

namespace FoodApp.Controllers
{
    public class MealPackageController : Controller
    {
        private readonly FoodAppDbContext _context;
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICanteenRepo _canteenRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly IMealPackageService _mealPackageService;

        public MealPackageController(FoodAppDbContext foodAppDbContext, IMealPackageRepo mealPackageRepo, IMealPackageService mealPackageService,
            IProductRepo productRepo, ICanteenRepo canteenRepo, IStudentRepo studentRepo)
        {
            _context = foodAppDbContext;
            _mealPackageRepo = mealPackageRepo;
            _mealPackageService = mealPackageService;
            _productRepo = productRepo;
            _canteenRepo = canteenRepo;
            _studentRepo = studentRepo;
        }

        [HttpGet]
        public IActionResult MealOverview()
        {
            var mealPackage = _mealPackageRepo.GetAvailableMealPackages();
            var canteens = _canteenRepo.GetCanteens();
            var students = _studentRepo.GetStudents();

            ViewBag.Students = students;
            ViewBag.Canteens = canteens;
            return View(mealPackage);
        }

        [HttpGet]
        public IActionResult Reserved()
        {
            var mealPackage = _mealPackageRepo.GetReservedMealPackages();

            var canteens = _canteenRepo.GetCanteens();
            var students = _studentRepo.GetStudents();

            ViewBag.Students = students;
            ViewBag.Canteens = canteens;
            return View(mealPackage);
        }

        [HttpGet]
        public IActionResult AddMealPackage()
        {
            var products = _productRepo.GetProducts();
            var canteens = _canteenRepo.GetCanteens();

            var mealPackageViewModel = new MealPackageViewModel
            {
                Products = products.ToList(),
                PickUpDateTime = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute),
                ExpireDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
            };

            ViewBag.Canteens = canteens;

            return View(mealPackageViewModel);
        }

        [HttpGet]
        public IActionResult EditMealPackage(int id)
        {
            try
            {
                var mealPackage = _mealPackageRepo.GetMealPackageById(id);

                if (mealPackage == null)
                {
                    return NotFound();
                }

                var products = _productRepo.GetProducts();
                var selectedProductIds = _context.MealPackages
                    .Where(mp => mp.Id == id)
                    .SelectMany(mp => mp.Products.Select(p => p.Id))
                    .ToList();

                // Bouw een lijst met objecten op die producten en een indicator voor selectie bevatten
                var productViewModels = products.Select(p => new ProductCheckBoxes
                {
                    Product = p,
                    IsSelected = selectedProductIds.Contains(p.Id),
                    IsChecked = selectedProductIds.Contains(p.Id)
                }).ToList();

                var canteens = _canteenRepo.GetCanteens();

                var mealPackageViewModel = new MealPackageViewModel
                {
                    Id = mealPackage.Id,
                    Name = mealPackage.Name,
                    PickUpDateTime = mealPackage.PickUpDateTime,
                    ExpireDateTime = mealPackage.ExpireDateTime,
                    AdultsOnly = mealPackage.AdultsOnly,
                    Price = mealPackage.Price,
                    City = mealPackage.City,
                    CanteenId = mealPackage.Canteen.Id,
                    MealType = mealPackage.MealType,
                    ProductCheckBoxes = productViewModels,

                    // Hier wordt de lijst met geselecteerde producten ingesteld
                    SelectedProducts = selectedProductIds
                };

                ViewBag.Canteens = canteens;

                return View(mealPackageViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while loading the meal package: " + ex.Message);
                return View("EditMealPackage", new MealPackageViewModel());
            }
        }

        [HttpPost]
        public IActionResult AddMealPackage(MealPackageViewModel mealPackageViewModel, List<int>? SelectedProducts)
        {
            try
            {
                var canteen = _canteenRepo.GetCanteenById(mealPackageViewModel.CanteenId);

                var currentTime = DateTime.Now;
                var timeDifference = mealPackageViewModel.PickUpDateTime - currentTime;

                var products = _productRepo.GetProducts();

                var selectedProducts = SelectedProducts != null
                    ? products.Where(p => SelectedProducts.Contains(p.Id)).ToList()
                    : new List<Product>();

                if (ModelState.IsValid)
                {
                    if (timeDifference.TotalHours < 48 && mealPackageViewModel.PickUpDateTime >= currentTime)
                    {
                        var mealPackage = new MealPackage
                        {
                            Name = mealPackageViewModel.Name,
                            PickUpDateTime = mealPackageViewModel.PickUpDateTime,
                            ExpireDateTime = mealPackageViewModel.ExpireDateTime,
                            AdultsOnly = false,
                            Price = mealPackageViewModel.Price,
                            City = canteen.City,
                            Canteen = canteen,
                            MealType = mealPackageViewModel.MealType,
                            Products = selectedProducts
                        };

                        _mealPackageRepo.AddMealPackage(mealPackage);

                        return RedirectToAction("MealOverview");
                    }
                    else
                    {
                        ModelState.AddModelError("PickUpDateTime", "PickUp DateTime must be within 2 days from now!");
                    }
                }


                if (selectedProducts.Count == 0)
                {
                    ModelState.AddModelError("SelectedProducts", "Please select at least one product.");
                }

                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

                mealPackageViewModel.Products = products.ToList();

                return View(mealPackageViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while adding a new MealPackage: " + ex.Message);
                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

                return View(mealPackageViewModel);
            }
        }

        [HttpPost]
        public IActionResult DeleteMealPackage(int id)
        {
            _mealPackageRepo.DeleteMealPackage(id);
            return RedirectToAction("MealOverview");
        }

        [HttpPost]
        public IActionResult EditMealPackage(MealPackageViewModel editMealPackageViewModel, List<int>? SelectedProducts)
        {
            try
            {
                var canteen = _canteenRepo.GetCanteenById(editMealPackageViewModel.CanteenId);
                var currentTime = DateTime.Now;
                var timeDifference = editMealPackageViewModel.PickUpDateTime - currentTime;

                var products = _productRepo.GetProducts();

                var selectedProducts = SelectedProducts != null
                    ? products.Where(p => SelectedProducts.Contains(p.Id)).ToList()
                    : new List<Product>();

                // Haal de lijst met producten op basis van de geselecteerde IDs
                editMealPackageViewModel.Products = selectedProducts;

                if (ModelState.IsValid)
                {
                    if (timeDifference.TotalHours < 48 && editMealPackageViewModel.PickUpDateTime >= currentTime)
                    {
                        var existingMealPackage = _mealPackageRepo.GetMealPackageById(editMealPackageViewModel.Id);

                        if (existingMealPackage != null)
                        {
                            existingMealPackage.Name = editMealPackageViewModel.Name;
                            existingMealPackage.PickUpDateTime = editMealPackageViewModel.PickUpDateTime;
                            existingMealPackage.ExpireDateTime = editMealPackageViewModel.ExpireDateTime;
                            existingMealPackage.Price = editMealPackageViewModel.Price;
                            existingMealPackage.MealType = editMealPackageViewModel.MealType;
                            existingMealPackage.AdultsOnly = false;

                            if (existingMealPackage.ExpireDateTime > existingMealPackage.PickUpDateTime)
                            {
                                // Als alles in orde is, bewerk het maaltijdpakket en keer terug naar de overzichtspagina
                                _mealPackageRepo.EditMealPackage(existingMealPackage);
                                return RedirectToAction("MealOverview");
                            }
                            else
                            {
                                ModelState.AddModelError("ExpireDateTime", "Expire DateTime must be later than PickUp DateTime!");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PickUpDateTime", "PickUp DateTime must be within 2 days from now!");
                    }
                }

                // Als ModelState ongeldig is of als er fouten zijn opgetreden, blijft de lijst met producten behouden
                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

                editMealPackageViewModel.ProductCheckBoxes = products.Select(p => new ProductCheckBoxes
                {
                    Product = p,
                    IsSelected = SelectedProducts != null && SelectedProducts.Contains(p.Id),
                    IsChecked = SelectedProducts != null && SelectedProducts.Contains(p.Id)
                }).ToList();

                return View(editMealPackageViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while updating the package: " + ex.Message);
                return View(editMealPackageViewModel);
            }
        }

    }
}
