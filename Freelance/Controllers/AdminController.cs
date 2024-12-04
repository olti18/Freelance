using AutoMapper;
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

namespace Freelance.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly FreelanceDbContext context;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IMapper mapper;
		private readonly IProjectPostRepository projectPostRepository;
		public AdminController(FreelanceDbContext context, UserManager<IdentityUser> userManager, IMapper mapper, IProjectPostRepository projectPostRepository)
		{
			this.context = context;
			this.userManager = userManager;
			this.mapper = mapper;
			this.projectPostRepository = projectPostRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProjectPosts()
		{
			var projectPosts = await context.ProjectPosts.
				Include(p => p.Proposals)
				.ToListAsync();

			return Ok(projectPosts);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProjectProposal(Guid id)
		{
			
			var projectPost = await context.ProjectPosts
				.Include(pp => pp.Proposals) 
				.FirstOrDefaultAsync(pp => pp.Id == id);

			if (projectPost == null)
			{
				return NotFound();
			}

			if (projectPost.Proposals != null && projectPost.Proposals.Any())
			{
				context.Proposals.RemoveRange(projectPost.Proposals);
			}

			context.ProjectPosts.Remove(projectPost);

			await context.SaveChangesAsync();

			return Ok("The project was removed successfully");
		}



	}
}
