using Microsoft.AspNetCore.Identity;

namespace Freelance.Models.Domain
{	
	public class Rating
	{
		public Guid RatingId { get; set; } // Primary key
		public int Score { get; set; } // 1 to 5
		public string? Comment { get; set; } // Optional feedback

		// Foreign Keys
		public string UserId { get; set; } // Freelancer receiving the rating
		public Guid ProjectPostId { get; set; } // Related job post
		public DateTime CreatedDate { get; set; } = DateTime.Now; // Date of rating
																  // Navigation Properties
		public IdentityUser User { get; set; } // Freelancer
		public ProjectPost ProjectPost { get; set; } // Job post
	}
}
