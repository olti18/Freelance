using AutoMapper;
using Freelance.Data;
using Freelance.Models.DTO.RatingDTO;
using Freelance.Repositories.IProjectPost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatingController : ControllerBase
	{
		private readonly FreelanceDbContext context;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IMapper mapper;
		private readonly IProjectPostRepository projectPostRepository;
		public RatingController(FreelanceDbContext context, UserManager<IdentityUser> userManager, IMapper mapper, IProjectPostRepository projectPostRepository)
		{
			this.context = context;
			this.userManager = userManager;
			this.mapper = mapper;
			this.projectPostRepository = projectPostRepository;
		}

		/*[HttpPost]
		public async Task<IActionResult> AddProposal([FromBody] AddRatingDto addRatingDto)
		{

		}*/
	}
}
