using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace Freelance.UI.Controllers
{
	public class AuthController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		
		public AuthController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
		}


		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var httpClient = _httpClientFactory.CreateClient();
			var jsonContent = JsonSerializer.Serialize(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync("https://localhost:7086/api/Auth/Login", content);

			if (response.IsSuccessStatusCode)
			{
				var responseData = await response.Content.ReadAsStringAsync();
				if (string.IsNullOrEmpty(responseData))
				{
					ModelState.AddModelError("", "Failed to retrieve a valid response from the API.");
					return View(model);
				}
				var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseData, options);

				//var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseData);
				if (loginResponse == null || string.IsNullOrEmpty(loginResponse.JwtToken))
				{
					ModelState.AddModelError("", "Failed to retrieve a valid JWT token.");
					return View(model);
				}

				HttpContext.Response.Cookies.Append("JwtToken", loginResponse.JwtToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = Request.IsHttps,
					SameSite = SameSiteMode.None,
					Expires = DateTimeOffset.UtcNow.AddHours(1)
				});


				//return RedirectToAction("Index", "Home");
				return RedirectToAction("index", "ProjectPost");
			}

			ModelState.AddModelError("", "Invalid username or password.");
			return View(model);
		}


		[HttpGet]
		public IActionResult Register()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterRequestDto model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var httpClient = _httpClientFactory.CreateClient();

			var jsonContent = JsonSerializer.Serialize(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync("https://localhost:7086/api/Auth/Register", content);

			if (response.IsSuccessStatusCode)
			{
				TempData["Message"] = "Registration successful! Please log in.";
				return RedirectToAction("Login");
			}

			ModelState.AddModelError("", "Registration failed. Please try again.");
			return View(model);
		}

	


	}
}
