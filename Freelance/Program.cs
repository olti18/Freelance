using Freelance.Data;
using Freelance.Mappings;
using Freelance.Repositories;
using Freelance.Repositories.IProjectPost;
using Freelance.Repositories.IProposal;
using Freelance.Repositories.IRating;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});

//Mos e prel
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = "localhost:6379"; // Replace with your Redis server address
	options.InstanceName = "MyAppRedisInstance";
});*/

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
	options.Cookie.SameSite = SameSiteMode.Lax;  // Allow cross-site cookies
});
//Mos e prek

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Freelance Api", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {

                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                    Scheme = "Oauth2",
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Db Context
builder.Services.AddDbContext<FreelanceDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("FreelanceConnectionString")));
// IRepository
builder.Services.AddScoped<IProjectPostRepository, SQLProjectPostRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IProposalRepository, SQLProposalRepository>();
builder.Services.AddScoped<IRatingRepository, SQLRatingRepository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
//Authentiaction
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("FreelanceWeb")
    .AddEntityFrameworkStores<FreelanceDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        //ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
