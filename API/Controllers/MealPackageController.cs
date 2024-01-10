using Application.Services;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPackageController : ControllerBase
    {
        private readonly IMealPackageService _mealPackageService;
        private readonly ILogger<MealPackageController> _logger;

        public MealPackageController(
            IMealPackageService mealPackageService,
            ILogger<MealPackageController> logger
            )
        {
            _mealPackageService = mealPackageService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetMealPackages()
        {
            _logger.LogInformation("GetMealPackages() aangeroepen");
            try
            {
                var mealPackages = _mealPackageService.GetMealPackages();
                return Ok(mealPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetAvailableMealPackages()
        {
            _logger.LogInformation("GetAvailableMealPackages() aangeroepen");
            try
            {
                var mealPackages = _mealPackageService.GetAvailableMealPackages();
                return Ok(mealPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("reserved")]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetReservedMealPackages()
        {
            _logger.LogInformation("GetReservedMealPackages() aangeroepen");
            try
            {
                var mealPackages = _mealPackageService.GetReservedMealPackages();
                return Ok(mealPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }


        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(MealPackage), 200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //[ProducesErrorResponseType(typeof(void))]
        //public IActionResult GetMealPackageById(int id)
        //{
        //    try
        //    {
        //        var mealPackage = _mealPackageRepository.GetMealPackageById(id);

        //        if (mealPackage == null)
        //        {
        //            return NotFound("MealPackage not found");
        //        }

        //        return Ok(mealPackage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet("reserved/student/{id}")]
        //[ProducesResponseType(typeof(MealPackage), 200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //[ProducesErrorResponseType(typeof(void))]
        //public IActionResult GetReservedByStudentId(int id)
        //{
        //    try
        //    {
        //        var mealPackage = _mealPackageRepository.GetReservedMealPackagesByStudent(id);

        //        if (mealPackage == null)
        //        {
        //            return NotFound("MealPackage not found");
        //        }

        //        return Ok(mealPackage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet("graphql")]
        //[ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        //[ProducesResponseType(500)]
        //public IActionResult GetAllMealPackagesGraphQL()
        //{
        //    var mealPackages = _mealPackageRepository.GetMealPackages();
        //    return Ok(new { Data = mealPackages });
        //}
        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(MealPackage), 200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //[ProducesErrorResponseType(typeof(void))]
        //public IActionResult GetMealPackageById(int id)
        //{
        //    try
        //    {
        //        var mealPackage = _mealPackageRepository.GetMealPackageById(id);

        //        if (mealPackage == null)
        //        {
        //            return NotFound("MealPackage not found");
        //        }

        //        return Ok(mealPackage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet("reserved/student/{id}")]
        //[ProducesResponseType(typeof(MealPackage), 200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //[ProducesErrorResponseType(typeof(void))]
        //public IActionResult GetReservedByStudentId(int id)
        //{
        //    try
        //    {
        //        var mealPackage = _mealPackageRepository.GetReservedMealPackagesByStudent(id);

        //        if (mealPackage == null)
        //        {
        //            return NotFound("MealPackage not found");
        //        }

        //        return Ok(mealPackage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet("graphql")]
        //[ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        //[ProducesResponseType(500)]
        //public IActionResult GetAllMealPackagesGraphQL()
        //{
        //    var mealPackages = _mealPackageRepository.GetMealPackages();
        //    return Ok(new { Data = mealPackages });
        //}

    }
}
