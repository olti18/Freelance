namespace Freelance.UI.Models
{
	public class RatingDto
	{
		public Guid RatingId { get; set; }
		public int Score { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedDate { get; set; }
		public string ProjectPostTitle { get; set; }
	}
}
