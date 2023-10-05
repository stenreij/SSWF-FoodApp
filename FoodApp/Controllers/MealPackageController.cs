using Application.Services;
using Core.Domain;
using Core.DomainServices;
using FoodApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace FoodApp.Controllers
{
    public class MealPackageController : Controller
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICanteenRepo _canteenRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly IMealPackageService _mealPackageService;

        public MealPackageController(IMealPackageRepo mealPackageRepo, IMealPackageService mealPackageService,
            IProductRepo productRepo, ICanteenRepo canteenRepo, IStudentRepo studentRepo)
        {
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
            var canteens = _canteenRepo.GetCanteens();

            ViewBag.Canteens = canteens;

            return View();
        }

        [HttpGet]
        public IActionResult EditMealPackage(int id)
        {
            var mealPackage = _mealPackageRepo.GetMealPackageById(id);
            var canteens = _canteenRepo.GetCanteens();

            var mealPackageViewModel = new MealPackageViewModel
            {
                Name = mealPackage.Name,
                PickUpDateTime = mealPackage.PickUpDateTime,
                ExpireDateTime = mealPackage.ExpireDateTime,
                AdultsOnly = mealPackage.AdultsOnly,
                Price = mealPackage.Price,
                City = mealPackage.City,
                CanteenId = mealPackage.Canteen.Id,
                MealType = mealPackage.MealType,
            };

            ViewBag.Canteens = canteens;
            return View(mealPackageViewModel);
        }

        [HttpPost]
        public IActionResult AddMealPackage(MealPackageViewModel mealPackageViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canteen = _canteenRepo.GetCanteenById(mealPackageViewModel.CanteenId);

                    var currentTime = DateTime.Now;
                    var timeDifference = mealPackageViewModel.PickUpDateTime - currentTime;

                    if (timeDifference.TotalHours < 48)
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
                        };

                        _mealPackageRepo.AddMealPackage(mealPackage);
                        return RedirectToAction("MealOverview");
                    }
                    else
                    {
                        ModelState.AddModelError("PickUpDateTime", "PickUpDateTime must be within 48 hours from now.");
                    }
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

        [HttpPost]
        public IActionResult DeleteMealPackage(int id)
        {
            _mealPackageRepo.DeleteMealPackage(id);
            return RedirectToAction("MealOverview");
        }

        [HttpPost]
        public IActionResult EditMealPackage(MealPackageViewModel editMealPackageViewModel)
        {
            var canteens = _canteenRepo.GetCanteens();
            ViewBag.Canteens = canteens;

            try
            {
                if (ModelState.IsValid)
                {
                    var canteen = _canteenRepo.GetCanteenById(editMealPackageViewModel.CanteenId);
                    var currentTime = DateTime.Now;

                    var timeDifference = editMealPackageViewModel.PickUpDateTime - currentTime;

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
                                _mealPackageRepo.EditMealPackage(existingMealPackage);
                                return RedirectToAction("MealOverview");
                            }
                            else
                            {
                                ModelState.AddModelError("ExpireDateTime", "Expired must be at a later time dan PickUpDate!");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PickUpDateTime", "PickUpDate must be within 2 days from now!");
                    }
                }

                var existingMealPackageFromDatabase = _mealPackageRepo.GetMealPackageById(editMealPackageViewModel.Id);

                if (existingMealPackageFromDatabase != null)
                {
                    editMealPackageViewModel.Name = existingMealPackageFromDatabase.Name;
                    editMealPackageViewModel.PickUpDateTime = existingMealPackageFromDatabase.PickUpDateTime;
                    editMealPackageViewModel.ExpireDateTime = existingMealPackageFromDatabase.ExpireDateTime;
                    editMealPackageViewModel.Price = existingMealPackageFromDatabase.Price;
                    editMealPackageViewModel.MealType = existingMealPackageFromDatabase.MealType;
                }

                return View("EditMealPackage", editMealPackageViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while updating the pakket: " + ex.Message);
                return View("EditMealPackage", editMealPackageViewModel);
            }
        }
    }
}
