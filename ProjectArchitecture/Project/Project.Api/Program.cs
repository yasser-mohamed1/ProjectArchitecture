using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project.EntityFramework.DataBaseContext;
using Project.Repository;
using Project.User.Services;
using Project.Comman.Idenitity;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moujam.Casiher.Comman.Models;

using Refit;
using Mujam.Intergration.Service.Mangment;
var builder = WebApplication.CreateBuilder(args);
var configuration= builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


#region Connection String
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


#endregion

#region Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 5;
}).AddDefaultTokenProviders()
.AddEntityFrameworkStores<ApplicationDbContext>();
#endregion
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(options =>
           {
               options.SaveToken = true;
               options.RequireHttpsMetadata = false;

               var validIssuers = configuration.GetSection("JWT:ValidIssuers").Get<string[]>();
               var validAudiences = configuration.GetSection("JWT:ValidAudiences").Get<string[]>();

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   //ValidIssuer = configuration["JWT:ValidIssur"],
                //   ValidIssuers = validIssuers,
                   ValidateAudience = false,
                   //ValidAudiences = validAudiences,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
               };
           });
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

});
var baseUrl = builder.Configuration.GetValue<string>("FileSettings:BASE_URL");
baseUrl = baseUrl.Remove(baseUrl.LastIndexOf("/"));
builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));
builder.Services.AddRefitClient<IServieMangamentApI>().ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
#region Dependency Injection
builder.Services.AddInfrastructureServices().
    AddReposetoriesServices();
#endregion

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
