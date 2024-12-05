using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Freelance.UI.Controllers
{
	public class AdminController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public AdminController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			DashboardStatsDto response = new DashboardStatsDto();
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];

			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Auth"); // Redirect to login if no token
			}

			var client = _httpClientFactory.CreateClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

			try
			{
				var httpResponseMessage = await client.GetAsync("https://localhost:7086/api/Admin/DashboardStats");

				httpResponseMessage.EnsureSuccessStatusCode();

				response = await httpResponseMessage.Content.ReadFromJsonAsync<DashboardStatsDto>();
			}
			catch (Exception ex)
			{
				// Log or handle the error
			}

			return View(response);
		}

		
	}
}
