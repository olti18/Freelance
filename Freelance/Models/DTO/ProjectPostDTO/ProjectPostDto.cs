namespace Freelance.Models.DTO.ProjectPostDto
{
    public class ProjectPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public Double Budget { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
