using Freelance.Models.Domain;

namespace Freelance.Repositories.IRating
{
	public class SQLRatingRepository : IRatingRepository
	{
		public Task<Rating> CreateRatingAsync(Rating proposal)
		{
			throw new NotImplementedException();
		}	
	}
}
