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
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepo _studentRepo;

        public StudentController(
            IStudentRepo studentRepo,
            ILogger<StudentController> logger
            )
        {
            _studentRepo = studentRepo;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetStudents()
        {
            _logger.LogInformation("GetStudents() called");
            try
            {
                var students = _studentRepo.GetStudents();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }
    }
}
