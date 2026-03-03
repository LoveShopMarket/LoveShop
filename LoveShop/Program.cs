using Identity;
using Identity.Constants;
using Identity.Models;
using Identity.Persistence;
using LoveShop.Handlers;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Services;
using LoveShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Category;
using Shared.DTOs.Product;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services
	.AddScoped<IGenericCrudService<Product, ProductDTO, ProductCreateDTO, ProductUpdateDTO>, ProductService>();

builder.Services
	.AddScoped<IGenericCrudService<Category, CategoryDTO, CategoryCreateDTO, CategoryUpdateDTO>, CategoryService>();

builder.Host.UseSerilog();

string? connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<LoveShopDbContext>(opt =>
	opt.UseNpgsql(connectionString));

builder.Services.AddIdentity(connectionString);

builder.Services.AddIdentityApiEndpoints<User>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi()
		.RequireAuthorization(Policies.RequireAdminRights);
}

app.MapGroup("api/Identity")
	.MapIdentityApi<User>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<LoveShopDbContext>();
	dbContext.Database.Migrate();

	var identityDbContext = scope.ServiceProvider.GetRequiredService<LoveShopIdentityDbContext>();
	identityDbContext.Database.Migrate();
}

app.UseSerilogRequestLogging();

app.UseCors();

app.Run();