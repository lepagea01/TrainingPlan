using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrainingPlan.ApplicationCore.Interfaces;
using TrainingPlan.ApplicationCore.Services;
using TrainingPlan.Infrastructure.Data;

namespace TrainingPlan.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddDbContext<TrainingPlanContext>(options =>
                options.UseInMemoryDatabase("TrainingPlan"));

            ConfigureServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<TrainingPlanContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DataConnection")));

            ConfigureServices(services);
        }

        public void ConfigureStagingServices(IServiceCollection services)
        {
            ConfigureProductionServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<TrainingPlanContext>(options =>
                options.UseInMemoryDatabase("TrainingPlan"));

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Composition Root
            services.TryAddScoped<IWorkoutService, WorkoutService>();
            services.TryAddScoped<IWorkoutRepository, WorkoutRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}