namespace Freelance.Models.DTO.RatingDTO
{
	public class AddRatingDto
	{
		/*public Guid ProposalId { get; set; }
		public int Score { get; set; } // 1 to 5
		public string? Comment { get; set; } // Optional feedback
		public string UserId { get; set; } // Freelancer receiving the rating
		public Guid ProjectPostId { get; set; } // Related job post
		public int Score { get; set; } // 1 to 5
		public string? Comment { get; set; } // Optional feedback
		public Guid UserId { get; set; } // Freelancer receiving the rating
		public Guid ProjectPostId { get; set; } // Related job post
		public Guid ProposalId { get; set; }*/
		public Guid ProposalId { get; set; } // The ID of the proposal being rated
		public Guid ProjectPostId { get; set; } // The ID of the related project post
		public Guid UserId { get; set; } // The ID of the freelancer receiving the rating
		public int Score { get; set; } // Rating score (e.g., 1 to 5)
		public string? Comment { get; set; } // Optional feedback
	}
}
