using AutoMapper;
using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProposalDTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Freelance.Repositories.IProposal
{
	public class SQLProposalRepository : IProposalRepository
	{
		private readonly FreelanceDbContext context;
		//private readonly ClaimsIdentity identity;
		private readonly IMapper mapper;
        public SQLProposalRepository(FreelanceDbContext context, IMapper mapper)
        {
            this.context = context;
			this.mapper = mapper;
        }

		public async Task<Proposal> CreateProposalAsync(Proposal proposal)
		{
			context.Proposals.Add(proposal);
			await context.SaveChangesAsync();

			return proposal; 
		}

		/*public async Task<List<Proposal?>> GetAllProposalAsync(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
			}

			var proposals = await context.ProjectPosts
				.Where(x => x.UserId == userId)
				.Include(p => p.Proposals)
				.ThenInclude(p => p.FreelancerId)
				.ToListAsync();

			return proposals;
		}*/

		public async Task<Proposal> DeleteProposalAsync(Guid id)
		{
			var proposal = await context.Proposals.FindAsync(id);

			if (proposal == null)
			{
				throw new ArgumentException($"The Proposal with this id={proposal} is null");
			}

			context.Proposals.Remove(proposal);
			await context.SaveChangesAsync();

			return proposal;
		}

		/*public async Task<List<Proposal?>> GetMyProposalAsync(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
			}

			var proposals = await context.Proposals
				.Where(p => p.FreelancerId == userId) // Assuming 'UserId' is the foreign key for proposals
				.Include(p => p.ProjectPost)    // Include related ProjectPost
				.ToListAsync();

			return proposals;
		}*/

		public async Task<Proposal?> UpdateProposalAsync(Guid proposalId, string userId, UpdateProposalDto updateProposalDto)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
			}

			var proposal = await context.Proposals
				.Include(p => p.ProjectPost)
				.FirstOrDefaultAsync(p => p.Id == proposalId);

			if (proposal == null)
			{
				return null;
			}

			if (proposal.FreelancerId != userId)
			{
				throw new UnauthorizedAccessException("You are not authorized to update this proposal.");
			}


			mapper.Map(updateProposalDto, proposal);

			await context.SaveChangesAsync();

			return proposal;
		}

	}
}
