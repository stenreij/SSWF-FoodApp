using Application.Services;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using API.RESTful.DTO;
using System.Security.Claims;

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
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(403)]
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
        [ServiceFilter(typeof(AuthFilter))]
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

        [HttpGet("Reserved/LoggedInUser")]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public IActionResult GetReservedMealPackagesForUser()
        {
            _logger.LogInformation("GetReservedMealPackagesForUser() called");

            try
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmailClaim))
                {
                    return Unauthorized("User is not authenticated.");
                }
                var user = _studentRepo.GetStudentByEmail(userEmailClaim);
                var userReservedMealPackages = _mealPackageRepo.GetReservedMealPackagesByStudent(user.Id);

                if (userReservedMealPackages == null || !userReservedMealPackages.Any())
                {
                    return NotFound("No reserved meal packages found for this user.");
                }

                return Ok(userReservedMealPackages);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, new { Success = false, Error = ex.Message });
            }
        }

        [HttpPost("Reserve")]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(200)]
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

        [HttpPost("CancelReservation")]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CancelReservation([FromBody] ReserveMealPackageRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { error = "Invalid JSON payload." });
                }

                var mealPackage = _mealPackageRepo.GetMealPackageById(request.mealPackageId);

                if (mealPackage == null)
                {
                    return BadRequest(new { error = "MealPackage not found." });
                }

                if (mealPackage.ReservedByStudent == null || mealPackage.ReservedByStudent.Id != request.studentId)
                {
                    return BadRequest(new { error = "This mealpackage is not reserved by this student." });
                }

                if (!_mealPackageService.CancelReservation(request.mealPackageId, request.studentId))
                {
                    return BadRequest(new { error = "Failed to cancel reservation. Please try again." });
                }

                return Ok(new { message = "Reservation successfully cancelled." });
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in CancelReservation: {e}");
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
