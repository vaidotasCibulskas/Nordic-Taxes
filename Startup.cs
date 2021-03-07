using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nordic.Taxes.Domain.Repositories;
using Nordic.Taxes.Domain.Services;
using Nordic.Taxes.Persistence.Contexts;
using Nordic.Taxes.Persistence.Repositories;
using Nordic.Taxes.Services;
using System.IO;

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

			services.AddControllers()
					  .AddNewtonsoftJson(options =>
					  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddAutoMapper(typeof(Startup));
			services.AddLogging(configure => configure.AddConsole());
			services.Configure<FormOptions>(o => {
				o.ValueLengthLimit = int.MaxValue;
				o.MultipartBodyLengthLimit = int.MaxValue; // Todo set limits for file size
				o.MemoryBufferThreshold = int.MaxValue;
			});
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseCors("CorsPolicy");

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
