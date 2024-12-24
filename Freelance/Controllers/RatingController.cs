using AutoMapper;
using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.RatingDTO;
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




		//[HttpPost("rate-freelancer")]
		//public async Task<IActionResult> RateFreelancer(Guid projectPostId, Guid proposalId)
		//{
		//	var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		//	// Validate ProjectPost ownership
		//	var projectPost = await context.ProjectPosts
		//		.Include(pp => pp.Proposals)
		//		.FirstOrDefaultAsync(pp => pp.Id == projectPostId && pp.UserId == loggedInUserId);

		//	if (projectPost == null)
		//		return Forbid("You can only rate freelancers for your own projects.");

		//	// Validate the selected proposal
		//	var proposal = projectPost.Proposals.FirstOrDefault(p => p.Id == proposalId && p.IsSelected);

		//	if (proposal == null)
		//		return BadRequest("Invalid proposal or freelancer not selected.");

		//	// Redirect to a rating form or handle the rating logic here
		//	// TempData["ProposalId"] = proposalId;
		//	// TempData["FreelancerId"] = proposal.FreelancerId;
		//	return RedirectToAction("RateForm", new { projectPostId, proposalId });
		//}



		//[HttpPost("submit-rating")]
		//public async Task<IActionResult> SubmitRating([FromForm] AddRatingDto ratingDto)
		//{
		//	// Validate input
		//	if (!ModelState.IsValid)
		//		return BadRequest(ModelState);

		//	var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		//	// Validate ProjectPost ownership
		//	var projectPost = await context.ProjectPosts
		//		.Include(pp => pp.Proposals)
		//		.FirstOrDefaultAsync(pp => pp.Id == ratingDto.ProjectPostId && pp.UserId == loggedInUserId);

		//	if (projectPost == null)
		//		return Forbid("You can only rate freelancers for your own project posts.");

		//	// Validate the Proposal
		//	var proposal = projectPost.Proposals.FirstOrDefault(p => p.Id == ratingDto.ProposalId && p.IsSelected);
		//	if (proposal == null)
		//		return BadRequest("Invalid proposal or freelancer not selected.");

		//	// Add Rating
		//	var rating = new Rating
		//	{
		//		RatingId = Guid.NewGuid(),
		//		Score = ratingDto.Score,
		//		Comment = ratingDto.Comment,
		//		UserId = ratingDto.UserId.ToString(),
		//		ProjectPostId = ratingDto.ProjectPostId
		//	};

		//	context.Ratings.Add(rating);
		//	await context.SaveChangesAsync();

		//	//TempData["SuccessMessage"] = "Rating submitted successfully!";
		//	return RedirectToAction("MyProposals");
		//}






		[HttpPost("rate-freelancer")]
		[Authorize]
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
