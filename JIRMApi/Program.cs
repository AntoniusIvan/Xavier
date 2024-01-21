using JIRMApi.Data;
using JIRMDataManager.Library.DataAccess;
using JIRMDataManager.Library.DataAccess.SystemCoreDataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Set the default culture using neutral culture (invariant culture)
var cultureInfo = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


//Personal Services
builder.Services.AddTransient<IInventoryData, InventoryData>();
builder.Services.AddTransient<IProductData, ProductData>();
builder.Services.AddTransient<ISaleData, SaleData>();
builder.Services.AddTransient<IUserData, UserData>();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
    .AddJwtBearer(
      "JwtBearer", jwtBearerOptions =>
      {
          jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuerSigningKey = true,
              //have to move the mysecret to the appsettings.json
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aVerySecureAndLongKeyString1234567890")),
              ValidateIssuer = false,
              ValidateAudience = false,
              ValidateLifetime = false,
              ClockSkew = TimeSpan.FromMinutes(5)
          };
      }
    );

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("OpenCorsPolicy", opt =>
          opt.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());
});

builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc(
      "v1",
      new OpenApiInfo
      {
          Title = "JiCo Retail Manager API",
          Version = "v1"
      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();
app.UseCors("OpenCorsPolicy");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
