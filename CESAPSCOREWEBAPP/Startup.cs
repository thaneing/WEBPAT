using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Hubs;
using CESAPSCOREWEBAPP.Models;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Rotativa.AspNetCore;


namespace CESAPSCOREWEBAPP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
          
            });

    


            //Connect DB SQL Server
            services.AddDbContext<DatabaseContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection_sql_server")));
            services.AddDbContext<NAVContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Nav")));
            //Connect DB SQL Server
            services.AddDbContext<UpdateContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection_sql_server")));



            services.AddDbContext<NAVSuperContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("NavSuper")));
         

            // Create Database when Database not found
            //var serviceProvider = services.BuildServiceProvider();
            //DatabaseInit.INIT(serviceProvider);




            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();


         


            //JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(OptionsBuilderConfigurationExtensions => {
                OptionsBuilderConfigurationExtensions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });


            //Config Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
       
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

           

            //services.AddCors(options => {
            //    options.AddPolicy(MyAllowSpecificOrigins, builder => builder
            //     .AllowAnyHeader()
            //     .AllowAnyOrigin()
            //     .AllowAnyMethod());
            //});


            //Use Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

       


 


            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();



            //Enable Assessor
            services.AddHttpContextAccessor();

            //Use MVC
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;
                }

            }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
              .AddCookieTempDataProvider()
              .AddSessionStateTempDataProvider();



            //Return API Data MAP Controller
            services.AddControllers().AddRazorRuntimeCompilation().AddNewtonsoftJson(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;
                }

            }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddSessionStateTempDataProvider();



            services.AddRazorPages(options =>
            {
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
             .AddCookieTempDataProvider()
             .AddSessionStateTempDataProvider();

            // Register reporting services in an application's dependency injection container.
            services.AddDevExpressControls();

            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureReportDesigner(designerConfigurator => {
                    designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                });
            });



            //Enable SignalR
            services.AddSignalR();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();


            //Config Session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(100000);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IHostingEnvironment envHost)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
             
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            // Middleware that run after routing occurs. Usually the following appear here:
            app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseCors(MyAllowSpecificOrigins);

    
            app.UseStaticFiles();


            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")),
                RequestPath = "/Images",
                EnableDirectoryBrowsing = true
            });

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "File")),
                RequestPath = "/Files",
                EnableDirectoryBrowsing = true

            });



            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine("\\\\192.168.18.9\\erp")),
            //    RequestPath = "/erp",
            //    EnableDirectoryBrowsing = true
               

            //});

            



            var reportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1(reportDirectory));
            DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
            app.UseDevExpressControls();








            // Runs matching. An endpoint is selected and set on the HttpContext if a match is found.
            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseSession();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

     
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Accounts}/{action=Login}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
     

            });



            RotativaConfiguration.Setup(envHost);
            
        }
    }
}
