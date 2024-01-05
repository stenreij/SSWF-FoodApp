using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPackageController : ControllerBase
    {
        private readonly IMealPackageRepo _mealPackageRepository;

        public MealPackageController(IMealPackageRepo mealPackageRepository)
        {
            _mealPackageRepository = mealPackageRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetMealPackages()
        {
            var mealPackages = _mealPackageRepository.GetMealPackages();
            return Ok(mealPackages);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MealPackage), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesErrorResponseType(typeof(void))]
        public IActionResult GetMealPackageById(int id)
        {
            try
            {
                var mealPackage = _mealPackageRepository.GetMealPackageById(id);

                if (mealPackage == null)
                {
                    return NotFound("MealPackage not found");
                }

                return Ok(mealPackage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("reserved/student/{id}")]
        [ProducesResponseType(typeof(MealPackage), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesErrorResponseType(typeof(void))]
        public IActionResult GetReservedByStudentId(int id)
        {
            try
            {
                var mealPackage = _mealPackageRepository.GetReservedMealPackagesByStudent(id);

                if (mealPackage == null)
                {
                    return NotFound("MealPackage not found");
                }

                return Ok(mealPackage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("graphql")]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetAllMealPackagesGraphQL()
        {
            var mealPackages = _mealPackageRepository.GetMealPackages();
            return Ok(new { Data = mealPackages });
        }

    }
}
