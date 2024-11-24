using AutoMapper;
using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.RatingDTO;
using Freelance.Repositories.IProjectPost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Freelance.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatingController : ControllerBase
	{
		private readonly FreelanceDbContext context;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IMapper mapper;
		private readonly IProjectPostRepository projectPostRepository;
		public RatingController(FreelanceDbContext context, UserManager<IdentityUser> userManager, IMapper mapper, IProjectPostRepository projectPostRepository)
		{
			this.context = context;
			this.userManager = userManager;
			this.mapper = mapper;
			this.projectPostRepository = projectPostRepository;
		}

		/*[HttpPost]
		public async Task<IActionResult> RateFreelancer([FromBody] AddRatingDto addRatingDto)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (userId is null) 
			{
				return BadRequest("User is not autheticated or null");
			}

			

			var proposal = await context.Proposals
				.Include(p => p.Freelancer)
				.Include(p => p.ProjectPost)
				.FirstOrDefaultAsync(p => p.Id == addRatingDto.ProposalId && p.ProjectPost.UserId == userId);
			
			if (proposal == null || !proposal.IsSelected)
				return BadRequest("Rating can only be submitted for selected proposals of your own project posts.");

			var rating = mapper.Map<Rating>(addRatingDto);
			
			// Create and save the rating


			context.Ratings.Add(rating);
			await context.SaveChangesAsync();

			return Ok("Rating submitted successfully.");
		}*/
		[HttpPost("rate-freelancer")]
		public async Task<IActionResult> RateFreelancer([FromBody] AddRatingDto addRatingDto)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			// Ensure the ProjectPost belongs to the logged-in user
			var projectPost = await context.ProjectPosts
				.Include(pp => pp.Proposals)
				.FirstOrDefaultAsync(pp => pp.Id == addRatingDto.ProjectPostId && pp.UserId == userId);

			if (projectPost == null)
				return Forbid("You can only rate freelancers for your own project posts.");

			// Validate the Proposal and Freelancer
			var proposal = projectPost.Proposals
				.FirstOrDefault(p => p.Id == addRatingDto.ProposalId && p.IsSelected);

			if (proposal == null)
				return BadRequest("Invalid proposal or freelancer not selected.");

			// Ensure Freelancer (UserId) exists
			var user = await context.Users.FindAsync(proposal.FreelancerId);
			if (user == null)
				return BadRequest("The specified freelancer does not exist.");

			// Map and Save Rating
			var rating = mapper.Map<Rating>(addRatingDto);
			rating.UserId = proposal.FreelancerId; // Set the correct UserId
		

			context.Ratings.Add(rating);
			await context.SaveChangesAsync();

			return Ok("Rating submitted successfully.");
		}

	}
}
