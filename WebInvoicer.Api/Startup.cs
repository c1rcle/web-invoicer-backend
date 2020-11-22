using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebInvoicer.Api.Configurations;
using WebInvoicer.Api.Filters;
using WebInvoicer.Core;
using WebInvoicer.Core.Dtos;
using WebInvoicer.Core.Dtos.Counterparty;
using WebInvoicer.Core.Dtos.Employee;
using WebInvoicer.Core.Dtos.Invoice;
using WebInvoicer.Core.Dtos.Product;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Repositories;
using WebInvoicer.Core.Repositories.Data;
using WebInvoicer.Core.Services;

namespace WebInvoicer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = GetAppConfiguration();
            var allowedHosts = Configuration.GetSection("Cors:AllowedHosts").Get<string[]>();
            var allowedMethods = Configuration.GetSection("Cors:AllowedMethods").Get<string[]>();

            services.AddSingleton(config.EmailConfig);
            services.AddSingleton(config.TokenConfig);
            services.AddSingleton(config.GusConfig);

            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Mapping));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(allowedHosts)
                    .AllowAnyHeader()
                    .WithMethods(allowedMethods);
                });
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(AuthorizeActionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));
                options.Filters.Add(typeof(ValidateTokenFilter));
            });

            services.AddHttpClient();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IInvoiceItemService, InvoiceItemService>();
            services.AddTransient<IDataService<CreateCounterpartyDto, CounterpartyDto>,
                DataService<CreateCounterpartyDto, CounterpartyDto, Counterparty>>();
            services.AddTransient<IDataService<CreateEmployeeDto, EmployeeDto>,
                DataService<CreateEmployeeDto, EmployeeDto, Employee>>();
            services.AddTransient<IDataService<CreateProductDto, ProductDto>,
                DataService<CreateProductDto, ProductDto, Product>>();
            services.AddTransient<IDataService<CreateInvoiceDto, InvoiceDto>,
                DataService<CreateInvoiceDto, InvoiceDto, Invoice>>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IInvoiceItemRepository, InvoiceItemRepository>();
            services.AddTransient<IDataRepository<Counterparty>, CounterpartyRepository>();
            services.AddTransient<IDataRepository<Employee>, EmployeeRepository>();
            services.AddTransient<IDataRepository<Product>, ProductRepository>();
            services.AddTransient<IDataRepository<Invoice>, InvoiceRepository>();

            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IGusService, GusService>();

            services.AddDbContextPool<DatabaseContext>(options =>
                options.UseMySql(config.ConnectionString));

            services.AddIdentityCore<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

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
                        .GetBytes(config.TokenConfig.JwtSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("1.0", new OpenApiInfo { Title = "Web Invoicer", Version = "1.0" });
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
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private AppConfiguration GetAppConfiguration()
        {
            var section = Environment.IsDevelopment()
                ? Configuration.GetSection("DevelopmentConfig")
                : Configuration.GetSection("ProductionConfig");

            return JsonSerializer.Deserialize<AppConfiguration>(section.Value);
        }
    }
}
