using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using TamsApi.Core.Identity;
using TamsApi.Data;
using TamsApi.Data.Seeds;
using TamsApi.Core.Tsas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.IIS;
using TamsApi.Core.Email;
using TamsApi.Core.Configuration;
using TsaData.Models;

namespace TamsApi
{
    public class Startup
    {
#if (DEBUG)
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
#endif

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISystemClock, SystemClock>();
            services.AddDbContext<ApplicationContext>(
                opts => opts.UseSqlServer(Configuration.GetConnectionString("ProjectMilano")));

            services.AddDbContextFactory<TsaContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("ProjectMilano")));

            services.AddHttpContextAccessor();
#if (DEBUG)
            services.AddCors(options =>
            {
                options.AddPolicy(name: "allowSpecificOrigins",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:8080")//.WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType)       // For intercepting and injecting Authorization token for SyncFusion Uploader component
                                                                                  .AllowAnyHeader()
                                                                                  .AllowAnyMethod()
                                                                                  .AllowCredentials()
                                                                                  .WithExposedHeaders("Content-Disposition");   // For downloading file & injecting Authorization token with simulating <a> downloading and getting the filename from "content-disposition"
                                  });
            });
#endif

            services.AddRazorPages();
            services.AddRepositories();
            services.Configure<SeedOptions>(Configuration.GetSection(SeedOptions.ConfigKey));
            services.Configure<IISServerOptions>(opts => {
                // This is necessary to ensure that IIS sets the WindowsPrincipal
                // before our Hybrid authentication handler is invoked. No principal
                // will exist otherwise.
                opts.AutomaticAuthentication = true;
            });
            services.Configure<ReportSettings>(Configuration.GetSection("ReportSettings"));

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = IISServerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = HybridAuthenticationHandler.SchemeName;
            })
                .AddScheme<AuthenticationSchemeOptions, HybridAuthenticationHandler>(HybridAuthenticationHandler.SchemeName, opts => { });
            services.AddTransient<IAuthenticatedUserBuilder, AuthenticatedUserBuilder>();
            services.AddExcelFileHandling();
            services.ConfigureSmtpOptions(Configuration).AddEmailServices();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/api/error");
            }

            app.UseRouting();
#if (DEBUG)
            app.UseCors("allowSpecificOrigins");
#endif
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var builder = endpoints.MapControllers();
#if (DEBUG)
                builder.RequireCors("allowSpecificOrigins");
#endif
                builder.RequireAuthorization();
            });
        }
    }
}
