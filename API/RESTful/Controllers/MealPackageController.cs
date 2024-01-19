using Application.Services;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using API.RESTful.DTO;

namespace API.RESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPackageController : ControllerBase
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly ILogger<MealPackageController> _logger;
        private readonly IStudentRepo _studentRepo;
        private readonly ICanteenRepo _canteenRepo;
        private readonly IMealPackageService _mealPackageService;

        public MealPackageController(
            IMealPackageRepo mealPackageRepo,
            ILogger<MealPackageController> logger,
            IStudentRepo studentRepo,
            ICanteenRepo canteenRepo,
            IMealPackageService mealPackageService
            )
        {
            _mealPackageRepo = mealPackageRepo;
            _logger = logger;
            _studentRepo = studentRepo;
            _canteenRepo = canteenRepo;
            _mealPackageService = mealPackageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetMealPackages()
        {
            _logger.LogInformation("GetMealPackages() called");
            try
            {
                var mealPackages = _mealPackageRepo.GetMealPackages();
                return Ok(mealPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        [HttpGet("Reserved")]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetReservedMealPackages()
        {
            _logger.LogInformation("GetReservedMealPackages() called");
            try
            {
                var reservedMealPackages = _mealPackageRepo.GetReservedMealPackages();
                return Ok(reservedMealPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        [HttpPost("Reserve")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult ReserveMealPackage([FromBody] ReserveMealPackageRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { error = "Invalid JSON payload." });
                }

                var studentBd = _studentRepo.GetStudentById(request.studentId).BirthDate;
                var mealPackage = _mealPackageRepo.GetMealPackageById(request.mealPackageId);

                var reservationDate = mealPackage.PickUpDateTime.Date;

                var studentAge = DateTime.Now.Year - studentBd.Year;
                var mealPackageAge = mealPackage.AdultsOnly;
                var canteens = _canteenRepo.GetCanteens();

                var existingReservationDate = _mealPackageRepo.GetReservedMealPackagesByStudent(request.studentId)
                    .Where(mp => mp.PickUpDateTime.Date == reservationDate)
                    .ToList();

                if (mealPackageAge && studentAge < 18)
                {
                    return BadRequest(new { error = "You have to be at least 18 to reserve this mealpackage." });
                }

                if (existingReservationDate.Any())
                {
                    return BadRequest(new { error = "You already have reserved a mealpackage for this day." });
                }

                if (mealPackage.ReservedByStudent != null)
                {
                    return BadRequest(new { error = "This mealpackage is already reserved." });
                }

                if (!_mealPackageService.ReserveMealPackage(request.mealPackageId, request.studentId))
                {
                    return BadRequest(new { error = "This mealpackage is not available at this moment." });
                }

                return Ok(new { message = "Reservation successful." });
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in ReserveMealPackage: {e}");
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
