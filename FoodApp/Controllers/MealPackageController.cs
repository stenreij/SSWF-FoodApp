using Application.Services;
using Core.Domain;
using Core.DomainServices;
using FoodApp.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IEmployeeRepo _employeeRepo;

        public MealPackageController(FoodAppDbContext foodAppDbContext, IMealPackageRepo mealPackageRepo, IMealPackageService mealPackageService,
            IProductRepo productRepo, ICanteenRepo canteenRepo, IStudentRepo studentRepo, IEmployeeRepo employeeRepo)
        {
            _context = foodAppDbContext;
            _mealPackageRepo = mealPackageRepo;
            _mealPackageService = mealPackageService;
            _productRepo = productRepo;
            _canteenRepo = canteenRepo;
            _studentRepo = studentRepo;
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        [Authorize]
        public IActionResult MealOverview()
        {
            var mealPackages = _mealPackageRepo.GetAvailableMealPackages();
            var canteens = _canteenRepo.GetCanteens();
            var students = _studentRepo.GetStudents();

            if (User.IsInRole("student"))
            {
                var studentId = _studentRepo.GetStudentByEmail(User.Identity.Name).Id;
                ViewBag.studentId = studentId;
            }
            else if (User.IsInRole("employee"))
            {
                var employeeId = _employeeRepo.GetEmployeeByEmail(User.Identity.Name).Id;
                ViewBag.employeeId = employeeId;
            }

            ViewBag.Students = students;
            ViewBag.Canteens = canteens;
            return View(mealPackages);
        }


        [HttpGet]
        [Authorize(Roles = "student")]
        public IActionResult Reserved()
        {
            if (User == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var studentId = _studentRepo.GetStudentByEmail(User.Identity.Name).Id;
            var mealPackage = _mealPackageRepo.GetReservedMealPackages(studentId);

            var canteens = _canteenRepo.GetCanteens();
            var students = _studentRepo.GetStudents();

            ViewBag.studentId = studentId;
            ViewBag.Students = students;
            ViewBag.Canteens = canteens;
            return View(mealPackage);
        }

        [HttpGet]
        [Authorize(Roles = "student")]
        public IActionResult MealPackageDetails(int id)
        {
            try
            {
                var mealPackage = _mealPackageRepo.GetMealPackageById(id);
                var canteens = _canteenRepo.GetCanteens();
                var studentId = _studentRepo.GetStudentByEmail(User.Identity.Name).Id;
                ViewBag.studentId = studentId;

                if (mealPackage == null)
                {
                    return NotFound();
                }

                var mealPackageProducts = _mealPackageRepo.GetMealPackageProducts(mealPackage.Id);
                mealPackage.Products = mealPackageProducts.ToList();

                ViewBag.Canteens = canteens;
                return View(mealPackage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while loading the meal package for editing: " + ex.Message);
                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

                return View("MealOverview");
            }
        }

        [HttpGet]
        [Authorize(Roles = "employee")]
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
        [Authorize(Roles = "employee")]
        public IActionResult EditMealPackage(int id)
        {
            try
            {
                var mealPackage = _mealPackageRepo.GetMealPackageById(id);
                var canteens = _canteenRepo.GetCanteens();

                if (mealPackage == null)
                {
                    return NotFound();
                }

                var products = _productRepo.GetProducts();
                var selectedProductIds = _context.MealPackages
                    .Where(mp => mp.Id == id)
                    .SelectMany(mp => mp.Products.Select(p => p.Id))
                    .ToList();

                var productViewModels = products.Select(p => new ProductCheckBoxes
                {
                    Product = p,
                    IsSelected = selectedProductIds.Contains(p.Id),
                    IsChecked = selectedProductIds.Contains(p.Id)
                }).ToList();

                var mealPackageViewModel = new MealPackageViewModel
                {
                    Id = mealPackage.Id,
                    Name = mealPackage.Name,
                    CanteenId = mealPackage.Canteen.Id,
                    PickUpDateTime = mealPackage.PickUpDateTime,
                    ExpireDateTime = mealPackage.ExpireDateTime,
                    AdultsOnly = mealPackage.AdultsOnly,
                    Price = mealPackage.Price,
                    MealType = mealPackage.MealType,
                    Products = products.ToList(),
                    ProductCheckBoxes = productViewModels,
                    SelectedProducts = selectedProductIds,
                };

                ViewBag.Canteens = canteens;
                return View(mealPackageViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while loading the meal package for editing: " + ex.Message);
                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

                return View(new MealPackageViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult AddMealPackage(MealPackageViewModel mealPackageViewModel, List<int> selectedProducts)
        {
            try
            {
                var products = _productRepo.GetProducts();
                var canteen = _canteenRepo.GetCanteenById(mealPackageViewModel.CanteenId);
                var SelectedProducts = selectedProducts != null
                    ? products.Where(p => selectedProducts.Contains(p.Id)).ToList()
                    : new List<Product>();
                bool containsAlcohol = SelectedProducts.Any(p => p.ContainsAlcohol);

                mealPackageViewModel.SelectedProducts = SelectedProducts.Select(p => p.Id).ToList();

                if (ModelState.IsValid)
                {
                    var currentTime = DateTime.Now;
                    var timeDifference = mealPackageViewModel.PickUpDateTime - currentTime;

                    if (timeDifference.TotalHours < 48 && mealPackageViewModel.PickUpDateTime >= currentTime)
                    {
                        var mealpackage = new MealPackage
                        {
                            Name = mealPackageViewModel.Name,
                            PickUpDateTime = mealPackageViewModel.PickUpDateTime,
                            ExpireDateTime = mealPackageViewModel.ExpireDateTime,
                            Price = mealPackageViewModel.Price,
                            MealType = mealPackageViewModel.MealType,
                            City = canteen.City,
                            AdultsOnly = containsAlcohol,
                            Canteen = canteen,
                            Products = SelectedProducts,
                        };
                        if (mealPackageViewModel.ExpireDateTime > mealPackageViewModel.PickUpDateTime)
                        {
                            _mealPackageRepo.AddMealPackage(mealpackage);
                            return RedirectToAction("MealOverview");
                        }
                        else
                        {
                            ModelState.AddModelError("ExpireDateTime", "ExpireDate has to be after the PickUpDateTime.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PickUpDateTime", "PickUpDateTime can only be 2 days from now.");
                    }
                }
                var canteens = _canteenRepo.GetCanteens();
                mealPackageViewModel.Products = products.ToList();
                ViewBag.Canteens = canteens;
                return View(mealPackageViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Something went wrong while adding a new pakket: " + ex.Message);
                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;
                return View(mealPackageViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult DeleteMealPackage(int id)
        {
            _mealPackageRepo.DeleteMealPackage(id);
            return RedirectToAction("MealOverview");
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EditMealPackage(MealPackageViewModel mealPackageViewModel)
        {
            try
            {
                var products = _productRepo.GetProducts();
                mealPackageViewModel.Products = products.ToList();

                if (ModelState.IsValid)
                {
                    var canteen = _canteenRepo.GetCanteenById(mealPackageViewModel.CanteenId);

                    var existingMealPackage = _mealPackageRepo.GetMealPackageById(mealPackageViewModel.Id);

                    if (existingMealPackage == null)
                    {
                        return NotFound();
                    }

                    var currentMealPackageProducts = _mealPackageRepo.GetMealPackageProducts(existingMealPackage.Id);

                    var productsToRemove = currentMealPackageProducts
                        .Where(product => !mealPackageViewModel.SelectedProducts
                        .Contains(product.Id))
                        .ToList();

                    foreach (var productToRemove in productsToRemove)
                    {
                        _mealPackageRepo.RemoveProductsFromMealPackage(existingMealPackage.Id, productToRemove.Id);
                    }

                    var productsToAdd = mealPackageViewModel.SelectedProducts
                        .Where(productId => !currentMealPackageProducts
                        .Any(product => product.Id == productId))
                        .ToList();

                    foreach (var productId in productsToAdd)
                    {
                        _mealPackageRepo.AddProductToMealPackage(existingMealPackage.Id, productId);
                    }

                    bool containsAlcohol = products.Any(p => mealPackageViewModel.SelectedProducts.Contains(p.Id) && p.ContainsAlcohol);

                    existingMealPackage.Name = mealPackageViewModel.Name;
                    existingMealPackage.PickUpDateTime = mealPackageViewModel.PickUpDateTime;
                    existingMealPackage.ExpireDateTime = mealPackageViewModel.ExpireDateTime;
                    existingMealPackage.Price = mealPackageViewModel.Price;
                    existingMealPackage.MealType = mealPackageViewModel.MealType;
                    existingMealPackage.City = canteen.City;
                    existingMealPackage.AdultsOnly = containsAlcohol;
                    existingMealPackage.Canteen = canteen;

                    if (mealPackageViewModel.ExpireDateTime > mealPackageViewModel.PickUpDateTime)
                    {
                        _mealPackageRepo.EditMealPackage(existingMealPackage);
                        return RedirectToAction("MealOverview");
                    }
                    else
                    {
                        ModelState.AddModelError("ExpireDateTime", "ExpireDate has to be after the PickUpDateTime.");
                    }
                }

                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;

                return View(mealPackageViewModel);
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("CustomError", "An error occurred: " + ex.Message);
                var canteens = _canteenRepo.GetCanteens();
                ViewBag.Canteens = canteens;
                return View(mealPackageViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "student")]
        public IActionResult ReserveMealPackage(int mealPackageId, int studentId)
        {
            try
            {
                var mealPackage = _mealPackageRepo.GetMealPackageById(mealPackageId);
                var reservationDate = mealPackage.PickUpDateTime.Date;

                var existingReservationDate = _mealPackageRepo.GetReservedMealPackages(studentId)
                    .Where(mp => mp.PickUpDateTime.Date == reservationDate)
                    .ToList();

                if (existingReservationDate.Any())
                {
                    ModelState.AddModelError("CustomError", "You already have reserved a mealpackage for this day.");                  
                    return RedirectToAction("MealOverview");
                }

                if (!_mealPackageService.ReserveMealPackage(mealPackageId, studentId))
                {
                    ModelState.AddModelError("CustomError", "This mealpackage is not available at this moment.");
                    return RedirectToAction("MealOverview");
                }

                return RedirectToAction("Reserved");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("CustomError", e.Message);
                return RedirectToAction("MealOverview");
            }
        }

        [HttpPost]
        [Authorize(Roles = "student")]
        public IActionResult CancelReservation(int mealPackageId, int studentId)
        {
            var student = _studentRepo.GetStudentByEmail(User.Identity.Name);
            if (student.Id != studentId)
            {
                return RedirectToAction("Reserved");
            }

            var mealPackage = _mealPackageRepo.GetMealPackageById(mealPackageId);
            if (mealPackage.ReservedByStudent == null)
            {
                return RedirectToAction("Reserved");
            }

            if (mealPackage.ReservedByStudent.Id != studentId)
            {
                return RedirectToAction("Reserved");
            }

            mealPackage.ReservedByStudent = null;
            _mealPackageRepo.EditMealPackage(mealPackage);
            return RedirectToAction("Reserved");

        }

    }
}
