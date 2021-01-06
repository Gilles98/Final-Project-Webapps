using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Dokterspunt.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Models;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Identity.UI.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using Dokterspunt.ViewModels.ModelsForViewModels;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Dokterspunt.ViewModels;
using Dokterspunt.Repository;
using Dokterspunt.Data.UnitOfWork;
using Microsoft.OpenApi.Models;
using Dokterspunt.Helpers;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Dokterspunt
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
         

            services.AddDbContext<DokterspuntContext>(options =>
           options.UseSqlServer(Configuration["Connection"]));
            services.AddDefaultIdentity<LoggedInUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DokterspuntContext>();
           
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure jwt authentication
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication()
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0]}
                };
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });
       
        services.AddControllersWithViews().AddFluentValidation().AddNewtonsoftJson(x =>x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Set Default Culture to replace dot with comma as decimal marker.
            CultureInfo cultureInfoDutchBelgium = new CultureInfo("nl-BE");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfoDutchBelgium;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfoDutchBelgium;
            services.AddTransient<IValidator<CreatePraktijkModel>, CreatePraktijkValidator>();
            services.AddTransient<IValidator<UpdatePraktijkModel>, UpdatePraktijkValidator>();
            services.AddTransient<IValidator<CreateSpecialisatieModel>, CreateSpecialisatieValidator>();
            services.AddTransient<IValidator<CreateAfspraakViewModel>, CreateAfspraakValidator>();
            services.AddTransient<IValidator<UpdatePatiëntGegevensViewModel>, UpdatePatiëntGegevensValidator>();
            services.AddTransient<IValidator<Klacht>, ValidateKlacht>();
            services.AddTransient<IValidator<MedischDossierViewModel>, DossierValidator>();
            services.AddTransient<IValidator<KlachtBeheerViewModel>, KlachtBeheerValidator>();
            services.AddTransient<IValidator<CreateDokterModel>, CreateDokterValidatior>();
            services.AddTransient<IValidator<UpdateDokterModel>, UpdateDokterValidator>();
            services.AddRazorPages();
        }

        public async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            DokterspuntContext context = serviceProvider.GetRequiredService<DokterspuntContext>();
           
            IdentityResult result;

            //rollen aanmaken
            string[] rollen = { "Admin", "Dokter", "Patiënt" };

            foreach (string item in rollen)
            {
                if (!await roleManager.RoleExistsAsync(item))
                {
                    result = await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
            DBInitializer.Initializer(context);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mijn Allereerste API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            logger.LogInformation("Connection: " +  Configuration["Connection"]);
           // CreateUserRoles(serviceProvider).Wait();
        }
    }
}
