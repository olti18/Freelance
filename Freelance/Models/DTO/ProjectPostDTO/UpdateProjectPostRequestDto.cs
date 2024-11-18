namespace Freelance.Models.DTO.ProjectPostDTO
{
    public class UpdateProjectPostRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public Double Budget { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
