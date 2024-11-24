using Freelance.Models.Domain;

namespace Freelance.Repositories.IRating
{
	public interface IRatingRepository
	{
		Task<Rating> CreateRatingAsync(Rating proposal);
	}
}
