using Freelance.Data;
using Freelance.Models.DTO;
using Freelance.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
		private readonly FreelanceDbContext context;
		public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, FreelanceDbContext context)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.context = context;
        }

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
		{
			var identityUser = new IdentityUser
			{
				UserName = registerRequestDto.UserName,
				Email = registerRequestDto.UserName
			};

			var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

			if (identityResult.Succeeded)
			{
				// Add role to this user
				if (!string.IsNullOrWhiteSpace(registerRequestDto.Role))
				{
					identityResult = await userManager.AddToRoleAsync(identityUser, registerRequestDto.Role);

					if (identityResult.Succeeded)
					{
						return Ok("User was registered! Please log in.");
					}
				}
				else
				{
					return BadRequest("Role is required.");
				}
			}

			return BadRequest("Something went wrong!");
		}

        
		[HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResulkt = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResulkt)
                {
                    // Get roles
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);

                    }
                    // Create Token 
                }
            }
            return BadRequest("Username or Password incorrect");
        }   

        [HttpGet("Get the user who is loged in")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserId()
        {
            var claims = await userManager.GetUserAsync(User);
            return Ok(claims);
        }
    }
}
