using Core.DomainServices.Interfaces;
using FoodApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentRepo _studentRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IStudentRepo studentRepo, IEmployeeRepo employeeRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _studentRepo = studentRepo;
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    false
                );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.CustomError = "Email/password incorrect!";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isStudentEmail = _studentRepo.GetStudentByEmail(model.Email) != null;
                var isEmployeeEmail = _employeeRepo.GetEmployeeByEmail(model.Email) != null;

                if (isStudentEmail || isEmployeeEmail)
                {
                    var user = new IdentityUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (isStudentEmail)
                        {
                            var student = _studentRepo.GetStudentByEmail(model.Email);
                            var birthDate = student.BirthDate.Date;
                            var today = DateTime.Today;
                            var age = today.Year - birthDate.Year;

                            if (birthDate > today.AddYears(-age))
                            {
                                age--;
                            }

                            if (age >= 16)
                            {
                                await _userManager.AddToRoleAsync(user, "student");
                            }
                            else
                            {
                                await _userManager.DeleteAsync(user);
                                ViewBag.CustomError = "You have to be at least 16 to make an account.";
                                return View(model);
                            }
                        }
                        else if (isEmployeeEmail)
                        {
                            await _userManager.AddToRoleAsync(user, "employee");
                        }

                        return RedirectToAction("Login", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description.ToString());
                    }
                }
                else
                {
                    ViewBag.CustomError = "You have to be a student or employee at Avans to create an account.";
                    return View(model);
                }
            }
            ViewBag.CustomError = "Email/password incorrect!";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
