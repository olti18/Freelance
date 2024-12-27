using AutoMapper;
using Freelance.CostumActionFilters;
using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProjectPostDto;
using Freelance.Models.DTO.ProjectPostDTO;
using Freelance.Repositories.IProjectPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Freelance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPostController : ControllerBase
    {
        private readonly FreelanceDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;
        private readonly IProjectPostRepository projectPostRepository;
        public ProjectPostController(FreelanceDbContext context, UserManager<IdentityUser> userManager, IMapper mapper, IProjectPostRepository projectPostRepository)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.projectPostRepository = projectPostRepository;
        }

        //[HttpPost("Create-Project-Post")] po gjun error qitu
        [HttpPost]
        [Authorize]
		
		//[ValidateModel]
		public async Task<IActionResult> CreateAsync([FromBody] AddProjectPostDto addProjectPostDto)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) { return Unauthorized("User ID claim not found."); }

            var projectPostDomainModel = mapper.Map<ProjectPost>(addProjectPostDto);
            projectPostDomainModel.UserId = user?.Id;

            await projectPostRepository.CreateAsync(projectPostDomainModel);

            return Ok(mapper.Map<ProjectPostDto>(projectPostDomainModel));
        }

		//[HttpGet]
		[HttpGet("MyProjects")]
		public async Task<IActionResult> GetMyProjects()
        {

			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var projects = await projectPostRepository.GetMyProjectsAsync(userId);

			return Ok(projects);
		}
        
        [HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllProjects()
        {
            var projects = await projectPostRepository.GetProjectsAsync();

            return Ok(projects);
        }
        //[HttpDelete("{id}")]
		[HttpDelete("MyProjects/{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) { return Unauthorized(); }

            var projectPost = await context.ProjectPosts.FindAsync(id);
            if (projectPost == null)
            {
                return NotFound("Project post not found.");
            }

            // Check if the logged-in user is the owner of the post
            if (projectPost.UserId != userId)
            {
                return Forbid("You are not authorized to delete this project post.");
            }

            await projectPostRepository.DeleteAsync(id);
            return Ok(mapper.Map<ProjectPostDto>(projectPost));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateProjectPostRequestDto updateDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if(userId == null) { return Unauthorized(); }

            var projectPost = await context.ProjectPosts.FindAsync(id);

            if (projectPost == null)
            {
                return NotFound("Project post not found.");
            }

            // Check if the logged-in user is the owner of the post
            if (projectPost.UserId != userId)
            {
                return Forbid("You are not authorized to update this project post.");
            }
            mapper.Map(updateDto,projectPost);

            await projectPostRepository.UpdateAsync(id,projectPost);

            return Ok("Project updated successfully");

        }


    }
}
