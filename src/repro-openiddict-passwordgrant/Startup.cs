using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using repro_openiddict_passwordgrant.Models;

namespace repro_openiddict_passwordgrant
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Add the database context, defaults to scoped; new context for each request.
            services.AddDbContext<ApplicationDbContext>(
                options => { options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); });

            // Adds Identity to IoC and confugres with the database.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict<ApplicationUser, ApplicationDbContext>()
                .EnableTokenEndpoint("/auth/token") // Password grant route
                .AllowPasswordFlow() // Enables password grant
                .DisableHttpsRequirement()
                .AddEphemeralSigningKey();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            using (var scope = scopeFactory.CreateScope())
            {
                // Conveniently create a user and create the database.
                MigrateAndCreateUser(scope).Wait();
            }

            app.UseIdentity(); // Authorization using ASP Identity.
            app.UseOAuthValidation();
            app.UseOpenIddict(); // OpenIddict takes care of the token issuing.

            app.UseMvc();
        }

        /// <summary>Initialises admin user and roles.</summary>
        /// <param name="scope">DI Scope.</param>
        private async Task MigrateAndCreateUser(IServiceScope scope)
        {
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            db.Database.Migrate();

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            if (await userManager.FindByNameAsync("testuser") == null)
            {
                var adminUser = new ApplicationUser {UserName = "testuser", PhoneNumberConfirmed = true};
                await userManager.CreateAsync(adminUser, "1234abcABC!!");
            }
        }
    }
}