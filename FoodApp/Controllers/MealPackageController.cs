using Application.Services;
using Core.Domain;
using Core.DomainServices;
using FoodApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodApp.Controllers
{
    public class MealPackageController : Controller
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly IProductRepo _productRepo;
        private readonly IMealPackageService _mealPackageService;

        public MealPackageController(IMealPackageRepo mealPackageRepo, IMealPackageService mealPackageService, 
            IProductRepo productRepo)
        {
            _mealPackageRepo = mealPackageRepo;
            _mealPackageService = mealPackageService;
            _productRepo = productRepo;
        }

        [HttpGet]
        public IActionResult AddMealPackage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMealPackage(MealPackageViewModel mealPackageViewModel)
        {
            if (ModelState.IsValid)
            {
                var mealPackage = new MealPackage
                {
                    Name = mealPackageViewModel.Name,
                    PickUpDateTime = mealPackageViewModel.PickUpDateTime,
                    ExpireDateTime = mealPackageViewModel.ExpireDateTime,
                    AdultsOnly = false,
                    Price = mealPackageViewModel.Price,
                    City = mealPackageViewModel.City,
                    CanteenId = mealPackageViewModel.CanteenId,
                    MealType = mealPackageViewModel.MealType,
                };

                _mealPackageRepo.AddMealPackage(mealPackage);
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
    }
}
