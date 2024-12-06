using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Freelance.UI.Controllers
{
	public class ProjectPostController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProjectPostController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			List<ProjectPostDto> response = new List<ProjectPostDto>();
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Auth"); // Redirect to login if no token
			}
			var client = _httpClientFactory.CreateClient();
			//var httpClient = _httpClientFactory.CreateClient();

			// Add the Authorization header
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

			try
			{
				//var client = _httpClientFactory.CreateClient();
				var httpResonseMessage = await client.GetAsync("https://localhost:7086/api/ProjectPost");

				httpResonseMessage.EnsureSuccessStatusCode();

				response.AddRange(await httpResonseMessage.Content.ReadFromJsonAsync<IEnumerable<ProjectPostDto>>());
			}
			catch (Exception ex)
			{

			}

			return View(response);

		}



		//[HttpGet]
		[HttpGet("Projects/MyProjects")]
		public async Task<IActionResult> MyProjects()
		{
			List<MyProjectsPostDto> response = new List<MyProjectsPostDto>();

		
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Auth"); 
			}

			try
			{
				
				var client = _httpClientFactory.CreateClient();

				
				client.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

				// Thirr API-n për të marrë projektet ************** Specifiko Routin************************
				var responseMessage = await client.GetAsync("https://localhost:7086/api/ProjectPost/MyProjects");

				// Kontrollo nëse përgjigja është e suksesshme
				if (!responseMessage.IsSuccessStatusCode)
				{
					TempData["ErrorMessage"] = "Failed to load your projects. Please try again.";
					return View(response); // Rikthe view me listë bosh
				}

				// Lexo të dhënat nga përgjigja
				response = await responseMessage.Content.ReadFromJsonAsync<List<MyProjectsPostDto>>();
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "An error occurred while loading your projects.";
			}

			return View(response);
		}

		[HttpGet]
		public IActionResult CreateProject()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateProject(AddProjectPostDto model)
		{
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Auth"); // Redirect to login if no token
			}

			var client = _httpClientFactory.CreateClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

			var httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7086/api/ProjectPost"),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};

			try
			{
				var httpResponseMessage = await client.SendAsync(httpRequestMessage);

				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
					Console.WriteLine($"Error: {httpResponseMessage.StatusCode}, Details: {errorContent}");
					return StatusCode((int)httpResponseMessage.StatusCode, errorContent);
				}

				var response = await httpResponseMessage.Content.ReadFromJsonAsync<AddProjectPostDto>();
				if (response is null)
				{
					return BadRequest("Failed to create project post.");
				}
				return RedirectToAction("index", "ProjectPost");
				//return View(response);
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Request error: {ex.Message}");
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}

		




	}
}
