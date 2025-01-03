using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.RESTful.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.RESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(
            IEmployeeRepo employeeRepo,
            IStudentRepo studentRepo,
            ILogger<UserController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            _employeeRepo = employeeRepo;
            _studentRepo = studentRepo;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation($"Login attempt for email: {model.Email}");

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        false,
                        false
                    );

                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(model.Email);

                        _logger.LogInformation($"Login successful for email: {model.Email}");

                        if (user != null)
                        {
                            var token = GenerateJwtTokenAsync(user);
                            return Ok(new { Token = token });
                        }
                        else
                        {
                            _logger.LogError("User is null after successful login.");
                            return StatusCode(500, "Invalid login attempt, wrong email/password combination.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid login attempt.");
                        return StatusCode(500, "Invalid login attempt.");
                    }
                }

                _logger.LogWarning($"Invalid model state for login attempt for email: {model.Email}");
                return BadRequest("Invalid model state.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login: {ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            int userId = await GetCustomUserIdAsync(user.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<int> GetCustomUserIdAsync(string email)
        {
            Student student = _studentRepo.GetStudentByEmail(email);
            if (student != null)
            {
                return student.Id;
            }
            else
            {
                Employee employee = _employeeRepo.GetEmployeeByEmail(email);
                if (employee != null)
                {
                    return employee.Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout successful");
        }

        [HttpGet("LoggedInUser")]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<MealPackage>), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public IActionResult GetLoggedInUser()
        {
            _logger.LogInformation("GetLoggedInUser() called");

            try
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmailClaim))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var student = _studentRepo.GetStudentByEmail(userEmailClaim);
                if (student != null)
                {
                    return Ok(student);
                }

                var employee = _employeeRepo.GetEmployeeByEmail(userEmailClaim);
                if (employee != null)
                {
                    return Ok(employee);
                }

                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

    }
}