using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nordic.Taxes.Domain.Repositories;
using Nordic.Taxes.Domain.Services;
using Nordic.Taxes.Persistence.Contexts;
using Nordic.Taxes.Persistence.Repositories;
using Nordic.Taxes.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Nordic.Taxes
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("AppDbContext"));
			});

			services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
			services.AddScoped<ITaxRepository, TaxRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IMunicipalityService, MunicipalityService>();
			services.AddScoped<ITaxService, TaxService>();

			

			services.AddAutoMapper(typeof(Startup));
			services.AddLogging(configure => configure.AddConsole());
			services.Configure<FormOptions>(o =>
			{
				o.ValueLengthLimit = int.MaxValue;
				o.MultipartBodyLengthLimit = int.MaxValue; // Todo set limits for file size
				o.MemoryBufferThreshold = int.MaxValue;
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Nordic.Tax Demo",
					Description = "Nordic.Tax Demo for ValuesController",
					//Contact = new OpenApiContact()
					//{
					//	Name = "",
					//	Email = "...@gmail.com",
					//	Url = "www.google.com"
					//}
				});
				c.IncludeXmlComments(GetXmlCommentsPath());
			});

			services.AddControllers();
			
		}
		private static string GetXmlCommentsPath()
		{

			return String.Format(@"{0}\Nordic.Taxes.xml",
			AppDomain.CurrentDomain.BaseDirectory);

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseHttpsRedirection(); // Comment this line, when deployung as self-hosted service or deal with: https://stackoverflow.com/questions/18443181/configuring-ssl-on-asp-net-self-hosted-web-api

			app.UseCors("CorsPolicy");

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			});
		}
	}
}
