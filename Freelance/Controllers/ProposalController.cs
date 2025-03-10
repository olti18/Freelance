﻿using AutoMapper;
using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProposalDTO;
using Freelance.Repositories.IProjectPost;
using Freelance.Repositories.IProposal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Security.Claims;

namespace Freelance.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProposalController : ControllerBase
	{
		private readonly FreelanceDbContext _appDbContext;
		private readonly IMapper _mapper;
		private readonly IProposalRepository proposalRepository;
		public ProposalController(FreelanceDbContext appDbContext, IMapper mapper, IProposalRepository proposalRepository)
		{
			this._appDbContext = appDbContext;
			this._mapper = mapper;
			this.proposalRepository = proposalRepository;
		}

		[HttpPost]
		[Authorize]
	
		public async Task<IActionResult> CreateProposalAsync (AddProposalDto addProposalDto)
		{
			if (addProposalDto == null)	
			{
				return BadRequest("Proposal Data is Required");
			}

			var projectPostExists = await _appDbContext.ProjectPosts.AnyAsync(pp => pp.Id == addProposalDto.ProjectPostId);
			if (!projectPostExists)
			{
				return BadRequest("The specified ProjectPostId does not exist.");
			}
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value; // or User.FindFirstValue(ClaimTypes.NameIdentifier) if NameIdentifier is used
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized("User must be logged in to create a proposal.");
			}

			var proposal = _mapper.Map<Proposal>(addProposalDto);
			proposal.FreelancerId = userId; // Assuming FreelancerId is a GUID
			proposal.CreatedDate = DateTime.UtcNow;

			// Save the proposal using the repository
			var createdProposal = await proposalRepository.CreateProposalAsync(proposal);

			// Return 200 OK with the created proposal
			return Ok(new
			{
				message = "Proposal created successfully.",
				proposal = createdProposal
			});

		}

		
		[HttpGet("GetAllProposals")]
		[Authorize]
		public async Task<IActionResult> GetAllProposals()
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value; // or User.FindFirstValue(ClaimTypes.NameIdentifier) if NameIdentifier is 

		
			var projectsWithProposals = await _appDbContext.ProjectPosts
			.Where(p => p.UserId == userId) // Filtron projektet që përkasin përdoruesit
			.Select(p => new
			{
				ProjectId = p.Id,
				Title = p.Title,
				Description = p.Description,
				Author = p.Author,
				Budget = p.Budget,
				CreatedDate = p.CreatedDate,
				Proposals = p.Proposals.Select(pr => new
			{
				ProposalId = pr.Id,
				Content = pr.Content,
				ProposedAmount = pr.ProposedAmount,
				SubmittedDate = pr.SubmittedDate,
				FreelancerId = pr.FreelancerId,
				FreelancerName = pr.Freelancer.UserName,
			  IsSelected = pr.IsSelected
			}).ToList()
		})
			.ToListAsync();

			return Ok(projectsWithProposals);
		}

		

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProposal(Guid id, [FromBody] UpdateProposalDto updateProposalDto)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the currently authenticated user's ID

			if (string.IsNullOrEmpty(currentUserId))
			{
				return Unauthorized("User is not authenticated.");
			}

			var updatedProposal = await proposalRepository.UpdateProposalAsync(id, currentUserId, updateProposalDto);

			if (updatedProposal == null)
			{
				return NotFound("Proposal not found.");
			}
				
			return Ok(updatedProposal); // Return the updated proposal
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProposal(Guid id)
		{
			await proposalRepository.DeleteProposalAsync(id); 

			return Ok(id);
		}

		[HttpPost("accept-proposal/{proposalId}")]
		
		public async Task<IActionResult> AcceptProposal(Guid proposalId)
		{
			// Retrieve the proposal and associated ProjectPost
			var proposal = await _appDbContext.Proposals
				.Include(p => p.ProjectPost) // Include ProjectPost for validation
				.FirstOrDefaultAsync(p => p.Id == proposalId);

			if (proposal == null)
				return NotFound("Proposal not found.");

			// Ensure the logged-in user is the author of the ProjectPost
			var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (proposal.ProjectPost.UserId != loggedInUserId)
				return Unauthorized("You are not authorized to accept this proposal.");

			// Ensure the ProjectPost does not already have a selected proposal
			if (proposal.ProjectPost.SelectedProposalId != null)
				return BadRequest("A freelancer has already been selected for this project.");

			// Mark the proposal as selected
			proposal.IsSelected = true;

			// Update the ProjectPost with the selected proposal
			proposal.ProjectPost.SelectedProposalId = proposal.Id;

			// Save changes to the database
			await _appDbContext.SaveChangesAsync();

			return Ok(new { Message = "Proposal accepted successfully." });
		}

		[HttpGet("GetMyProposals")]
		[Authorize]
		public async Task<IActionResult> GetMyProposals()
		{
			// Merr UserId nga Claims
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized("User is not authenticated.");
			}

			// Merr propozimet nga baza e të dhënave
			var proposals = await _appDbContext.Proposals
		.Where(p => p.FreelancerId == userId)
		.Select(p => new ProjectWithProposalsDto
		{
			ProposalId = p.Id,
			Content = p.Content,
			ProposedAmount = p.ProposedAmount,
			SubmittedDate = p.SubmittedDate,
			CreatedDate = p.CreatedDate,
			IsSelected = p.IsSelected,
			Project = new ProjectDto
			{
				ProjectId = p.ProjectPost.Id,
				Title = p.ProjectPost.Title,
				Description = p.ProjectPost.Description,
				Budget = p.ProjectPost.Budget,
				Author = p.ProjectPost.Author,
				CreatedDate = p.ProjectPost.CreatedDate
			}
		})
		.ToListAsync();

			return Ok(proposals);
		}



	}
}
