using AutoMapper;
using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProposalDTO;
using Freelance.Repositories.IProjectPost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Freelance.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProposalController : ControllerBase
	{
		private readonly FreelanceDbContext _appDbContext;
		private readonly IMapper _mapper;

		public ProposalController(FreelanceDbContext appDbContext, IMapper mapper)
		{
			_appDbContext = appDbContext;
			_mapper = mapper;
		}

		// GET: api/proposal
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProposalDto>>> GetProposals()
		{
			var proposals = await _appDbContext.Proposals.ToListAsync();

			// Map the proposals to ProposalDto using AutoMapper
			var proposalDtos = _mapper.Map<List<ProposalDto>>(proposals);

			return Ok(proposalDtos);
		}

		// GET: api/proposal/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ProposalDto>> GetProposal(Guid id)
		{
			var proposal = await _appDbContext.Proposals.FindAsync(id);

			if (proposal == null)
			{
				return NotFound();
			}

			// Map the proposal to ProposalDto using AutoMapper
			var proposalDto = _mapper.Map<ProposalDto>(proposal);

			return Ok(proposalDto);
		}

		[HttpPost]
		public async Task<ActionResult<Proposal>> CreateProposal(AddProposalDto addProposalDto)
		{
			if (addProposalDto == null)
			{
				return BadRequest("Proposal data is required.");
			}

			// Check if the ProjectPostId exists
			var projectPostExists = await _appDbContext.ProjectPosts.AnyAsync(pp => pp.Id == addProposalDto.ProjectPostId);
			if (!projectPostExists)
			{
				return BadRequest("The specified ProjectPostId does not exist.");
			}

			// Extract FreelancerId from JWT claims
			var freelancerId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
			if (freelancerId == null)
			{
				return Unauthorized("Freelancer identity could not be determined.");
			}

			// Map DTO to Proposal model
			var proposal = _mapper.Map<Proposal>(addProposalDto);
			proposal.FreelancerId = freelancerId;
			proposal.CreatedDate = DateTime.UtcNow;

			// Save to the database
			_appDbContext.Proposals.Add(proposal);
			await _appDbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProposal), new { id = proposal.Id }, proposal);
		}


		// POST: api/proposal
		/*[HttpPost]
		public async Task<ActionResult<Proposal>> CreateProposal(AddProposalDto addProposalDto)
		{
			// Validate the incoming data
			if (addProposalDto == null)
			{
				return BadRequest("Proposal data is required.");
			}

			// Extract the FreelancerId from the current user (JWT claims)
			var freelancerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (freelancerId == null)
			{
				return Unauthorized("Freelancer identity could not be determined.");
			}

			// Map DTO to the Proposal domain model using AutoMapper
			var proposal = _mapper.Map<Proposal>(addProposalDto);
			proposal.FreelancerId = freelancerId; // Automatically assign the FreelancerId
			proposal.CreatedDate = DateTime.UtcNow; // Ensure the CreatedDate is set

			// Save the new proposal to the database
			_appDbContext.Proposals.Add(proposal);
			await _appDbContext.SaveChangesAsync();

			// Return the created proposal with status code 201 (Created)
			return CreatedAtAction(nameof(GetProposal), new { id = proposal.Id }, proposal);
		}
		*/

		// PUT: api/proposal/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProposal(Guid id, UpdateProposalDto updateProposalDto)
		{
			if (id != updateProposalDto.Id)
			{
				return BadRequest("Proposal ID mismatch.");
			}

			var proposal = await _appDbContext.Proposals.FindAsync(id);

			if (proposal == null)
			{
				return NotFound();
			}

			// Map UpdateProposalDto to Proposal entity
			_mapper.Map(updateProposalDto, proposal);

			// Save changes to DB
			_appDbContext.Entry(proposal).State = EntityState.Modified;
			await _appDbContext.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/proposal/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProposal(Guid id)
		{
			var proposal = await _appDbContext.Proposals.FindAsync(id);

			if (proposal == null)
			{
				return NotFound();
			}

			_appDbContext.Proposals.Remove(proposal);
			await _appDbContext.SaveChangesAsync();

			return NoContent();
		}

		[HttpGet("{projectId}/freelancers")]
		public async Task<ActionResult<List<FreelancerDto>>> GetFreelancersByProject(Guid projectId)
		{
			// Validate the project ID
			var projectExists = await _appDbContext.ProjectPosts.AnyAsync(p => p.Id == projectId);
			if (!projectExists)
			{
				return NotFound("Project not found.");
			}

			// Query for freelancers who proposed on this project
			var freelancers = await (from proposal in _appDbContext.Proposals
									 join user in _appDbContext.Users on proposal.FreelancerId equals user.Id
									 where proposal.ProjectPostId == projectId
									 select new FreelancerDto
									 {
										 Id = user.Id,
										 FullName = user.UserName, // Adjust if you have a specific FullName field
										 Email = user.Email,
									 }).ToListAsync();

			

			return Ok(freelancers);
		}


		/*[HttpGet("user/{userId}/proposals")]
		public async Task<ActionResult<IEnumerable<ProposalDto>>> GetProposalsForUserProjects(string userId)
		{
			// Get all project posts created by the user (client)
			var projectPosts = await _appDbContext.ProjectPosts
				.Where(p => p.UserId == userId) // Filter by the user's ID
				.ToListAsync();

			if (projectPosts == null || projectPosts.Count == 0)
			{
				return NotFound("No project posts found for this user.");
			}

			// Get all proposals associated with the project posts
			var proposals = await _appDbContext.Proposals
				.Where(p => projectPosts.Select(pp => pp.Id).Contains(p.ProjectPostId)) // Filter by project posts
				.ToListAsync();

			// Map the Proposals to ProposalDto using AutoMapper
			var proposalDtos = _mapper.Map<List<ProposalDto>>(proposals);

			return Ok(proposalDtos); // Return the mapped proposals
		}*/

	}
}
