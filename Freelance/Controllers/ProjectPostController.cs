using Freelance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Freelance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPostController : ControllerBase
    {
        private readonly FreelanceDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        public ProjectPostController(FreelanceDbContext context, UserManager<IdentityUser> userManager) 
        {
            this.context = context;
            this.userManager = userManager;
        }

        /*[HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectPosts()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return Ok("Email claim not found.");
            }


            return Ok($"Success {emailClaim}");
        }*/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserId()
        {
            var claims = await userManager.GetUserAsync(User);
            return Ok(claims);
        }
        /*[HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUserId()
        {
            var claims = await userManager.GetUserAsync(User);
            return Ok(claims);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserIid()
        {
            // Get the user from UserManager
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            var id = user.Id;
            // Return the User ID
            return Ok(new { UserId = user.Id });
        }*/
    }
}
