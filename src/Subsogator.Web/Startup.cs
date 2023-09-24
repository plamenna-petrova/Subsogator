using Data.DataAccess;
using Data.DataModels.Entities.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Subsogator.Infrastructure.Extensions;
using System.Globalization;
using static Subsogator.Common.GlobalConstants.IdentityConstants;

namespace Subsogator.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>();

            serviceCollection.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole($"{AdministratorRoleName}"));
                options.AddPolicy("AdministratorOrEditor", policy => 
                    policy.RequireRole($"{AdministratorRoleName}, {EditorRoleName}"
                ));
                options.AddPolicy("AdministratorEditorOrUploader", policy =>
                    policy.RequireRole($"{AdministratorRoleName}, {EditorRoleName}, {UploaderRoleName}"
                ));
            });

            serviceCollection.AddControllersWithViews();

            serviceCollection.AddRazorPages();

            serviceCollection.RegisterRepositories();

            serviceCollection.RegisterServiceLayer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();

                applicationBuilder.UseDatabaseErrorPage();

                applicationBuilder.MigrateDatabase(logger);

                applicationBuilder.ApplyDatabaseSeeding(logger);
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/Home/Error");
                applicationBuilder.UseHsts();
            }

            applicationBuilder.UseHttpsRedirection();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US")
            };

            applicationBuilder.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            applicationBuilder.UseStaticFiles();

            applicationBuilder.UseRouting();

            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
