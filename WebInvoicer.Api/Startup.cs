using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebInvoicer.Core;
using WebInvoicer.Core.Email;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Repositories;
using WebInvoicer.Core.Services;
using WebInvoicer.Core.Token;

namespace WebInvoicer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var configSection = GetConfigSection();

            var allowedHosts = Configuration.GetSection("Cors:AllowedHosts").Get<string[]>();
            var allowedMethods = Configuration.GetSection("Cors:AllowedMethods").Get<string[]>();

            services.Configure<EmailConfiguration>(configSection.GetSection("EmailConfig"));
            services.Configure<TokenConfiguration>(configSection.GetSection("TokenConfig"));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(allowedHosts)
                    .AllowAnyHeader()
                    .WithMethods(allowedMethods);
                });
            });

            services.AddControllers();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IEmailService, EmailService>();

            services.AddDbContextPool<DatabaseContext>(options =>
                options.UseMySql(configSection["ConnectionString"]));

            services.AddIdentityCore<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DatabaseContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(configSection["TokenConfig:JwtSecret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Invoicer", Version = "1.0" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/1.0/swagger.json", "Web Invoicer");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto
            });
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IConfigurationSection GetConfigSection()
        {
            return Environment.IsProduction()
                ? Configuration.GetSection("DevelopmentConfig")
                : Configuration.GetSection("ProductionConfig");
        }
    }
}
