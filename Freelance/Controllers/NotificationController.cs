using AutoMapper;
using Freelance.Data;
using Freelance.Repositories.IProjectPost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly FreelanceDbContext context;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IMapper mapper;
		private readonly IProjectPostRepository projectPostRepository;
		public NotificationController(FreelanceDbContext context, UserManager<IdentityUser> userManager, IMapper mapper, IProjectPostRepository projectPostRepository)
		{
			this.context = context;
			this.userManager = userManager;
			this.mapper = mapper;
			this.projectPostRepository = projectPostRepository;
		}
	}
}
