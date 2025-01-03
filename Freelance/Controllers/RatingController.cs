﻿using AutoMapper;
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




		[HttpGet("freelancer-ratings")]
		[Authorize]
		public async Task<IActionResult> GetFreelancerRatings()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in freelancer's ID

			if (string.IsNullOrEmpty(userId))
				return Unauthorized("User is not authenticated.");

			// Fetch ratings for the logged-in freelancer
			var ratings = await context.Ratings
				.Where(r => r.UserId == userId)
				.Select(r => new
				{
					r.RatingId,
					r.Score,
					r.Comment,
					r.CreatedDate,
					ProjectPostTitle = r.ProjectPost.Title // Assuming ProjectPost has a Title field
				})
				.ToListAsync();

			return Ok(ratings);
		}


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
