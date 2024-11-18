using Freelance.Data;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProposalDTO;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Repositories.IProposal
{
	public class SQLProposalRepository : IProposalRepository
	{
		private readonly FreelanceDbContext context;
        public SQLProposalRepository(FreelanceDbContext context)
        {
            this.context = context;
        }

		

	

		/*public Task<Proposal> GetProposals()
		{
			var proposals = await context.Proposals.ToListAsync();
			
			return 
		}*/
	}
}
