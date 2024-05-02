
using LocationTracker.Api.Services;
using LocationTracker.Api.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;

namespace LocationTracker.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddSingleton<IDataService, DataService>();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Location Tracker API",
				Description = "Records locations visited by users and enables querying of recorded data.",
				TermsOfService = new Uri("https://example.com/terms"),
				Contact = new OpenApiContact
				{
					Name = "Edward Noble",
					Url = new Uri("https://example.com/contact")
				},
				License = new OpenApiLicense
				{
					Name = "Example License",
					Url = new Uri("https://example.com/license")
				}
			}));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
