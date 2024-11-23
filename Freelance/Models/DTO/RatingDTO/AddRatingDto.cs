namespace Freelance.Models.DTO.RatingDTO
{
	public class AddRatingDto
	{
		public int Score { get; set; } // 1 to 5
		public string? Comment { get; set; } // Optional feedback
		public string UserId { get; set; } // Freelancer receiving the rating
		public Guid ProjectPostId { get; set; } // Related job post
	}
}
