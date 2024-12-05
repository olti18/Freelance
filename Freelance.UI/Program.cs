var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowAllOrigins", builder =>
//	{
//		builder.AllowAnyOrigin()
//			   .AllowAnyMethod()
//			   .AllowAnyHeader();
//	});
//});
// Add services to the container.
builder.Services.AddControllersWithViews();
//Added By Me
//Mos e prek
builder.Services.AddDistributedMemoryCache();
/*builder.Services.AddSession(options =>
{
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.SameSite = SameSiteMode.None;
});*/
builder.Services.AddAntiforgery(options =>
{
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Ensure antiforgery cookies are secure
	options.Cookie.SameSite = SameSiteMode.None;  // Allow cross-site cookies
});


//Mos e prek

//builder.Services.AddHttpClient("AuthApiClient", client =>
//{
//	client.BaseAddress = new Uri("https://localhost:7086/"); // Replace with your actual Web API base URL
//	client.DefaultRequestHeaders.Add("Accept", "application/json");
//});
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseSession();
app.UseRouting();
app.UseAuthorization();

//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Auth}/{action=login}/{id?}");

app.Run();
