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
		//private readonly ILogger _logger;
		public AuthController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
			//_logger = logger;
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

				/*HttpContext.Response.Cookies.Append("JwtToken", loginResponse.JwtToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.None,  // Allows cross-site usage of the cookie
					Expires = DateTimeOffset.UtcNow.AddHours(1)
				});*/

				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("", "Invalid username or password.");
			return View(model);
		}









		/*[HttpPost]
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
				var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseData);

				// Set the JWT token in an HTTP-only, secure cookie
				HttpContext.Response.Cookies.Append("JwtToken", loginResponse.JwtToken, new CookieOptions
				{
					HttpOnly = true, // Prevent JavaScript access for security
					Secure = true,   // Ensure the cookie is only sent over HTTPS
					Expires = DateTimeOffset.UtcNow.AddHours(1) // Set the cookie expiry
				});

				return RedirectToAction("Index", "Home"); // Redirect to home/dashboard page
			}

			ModelState.AddModelError("", "Invalid username or password.");
			return View(model);
		}*/


		/*[HttpPost]
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
				var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseData);

				// Set the JWT token in TempData
				TempData["JwtToken"] = loginResponse.JwtToken;

				return RedirectToAction("Index", "Home"); // Redirect to home/dashboard page
			}

			ModelState.AddModelError("", "Invalid username or password.");
			return View(model);
		}*/



		/*[HttpPost]
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
				var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseData);

				TempData["JwtToken"] = loginResponse.JwtToken;
				return RedirectToAction("Index", "Home"); // Redirect to home/dashboard
			}

			ModelState.AddModelError("", "Invalid username or password.");
			return View(model);
		}*/
	}
}
