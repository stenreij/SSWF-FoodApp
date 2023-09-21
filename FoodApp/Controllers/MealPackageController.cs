using Application.Services;
using Core.Domain;
using Core.DomainServices;
using FoodApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodApp.Controllers
{
    public class MealPackageController : Controller
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICanteenRepo _canteenRepo;
        private readonly IMealPackageService _mealPackageService;

        public MealPackageController(IMealPackageRepo mealPackageRepo, IMealPackageService mealPackageService,
            IProductRepo productRepo, ICanteenRepo canteenRepo)
        {
            _mealPackageRepo = mealPackageRepo;
            _mealPackageService = mealPackageService;
            _productRepo = productRepo;
            _canteenRepo = canteenRepo;
        }

        [HttpGet]
        public IActionResult MealOverview()
        {
            var mealPackage = _mealPackageRepo.GetMealPackages();
            var canteens = _canteenRepo.GetCanteens();

            ViewBag.Canteens = canteens;
            return View(mealPackage);
        }

        [HttpGet]
        public IActionResult Reserved()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddMealPackage()
        {
            var canteens = _canteenRepo.GetCanteens(); 

            ViewBag.Canteens = canteens; 

            return View();
        }

        [HttpPost]
        public IActionResult AddMealPackage(MealPackageViewModel mealPackageViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canteen = _canteenRepo.GetCanteenById(mealPackageViewModel.CanteenId);

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
                    };

                    _mealPackageRepo.AddMealPackage(mealPackage);

                    return RedirectToAction("MealOverview");
                }

                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

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
    }
}
