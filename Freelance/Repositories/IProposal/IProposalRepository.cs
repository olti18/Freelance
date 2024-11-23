using Freelance.Models.Domain;
using Freelance.Models.DTO.ProposalDTO;

namespace Freelance.Repositories.IProposal
{
	public interface IProposalRepository
	{
		//Task<List<Proposal?>> GetMyProposalAsync(string userId);
		Task<Proposal> CreateProposalAsync(Proposal proposal);
		Task<Proposal> UpdateProposalAsync(Guid proposalId, string userId, UpdateProposalDto updateProposalDto);
		Task<Proposal> DeleteProposalAsync(Guid proposalId);
		//
		//Task<List<Proposal?>> GetAllProposalAsync(string userId);
	}
}
