namespace Freelance.UI.Models
{
	public class ProjectPostDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public double Budget { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
