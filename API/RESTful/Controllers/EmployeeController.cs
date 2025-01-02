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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeRepo employeeRepo,
            ILogger<EmployeeController> logger
            )
        {
            _employeeRepo = employeeRepo;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetEmployees()
        {
            _logger.LogInformation("GetEmployees() called");
            try
            {
                var employees = _employeeRepo.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }
    }
}
