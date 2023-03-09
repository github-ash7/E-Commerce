using APurchaseService.Helpers;
using Contracts.IRepository;
using Contracts.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Repository;
using Services;
using System.Text;

namespace APurchaseService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) // Configure services 
        {
            services.AddHttpContextAccessor();

            services.AddTransient<ExceptionHandlingMiddleware>();

            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            }); ;

            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            services.AddHttpClient();

            services.AddAutoMapper(typeof(MapperProfile));

            // Registers the service for CommonService

            services.AddScoped<ICommonService, CommonService>();

            // Registers the service for "WishList" - service/repository

            services.AddScoped<IWishListService, WishListService>();
            services.AddScoped<IWishListRepository, WishListRepository>();

            // Registers the service for "Cart" - service/repository

            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartRepository, CartRepository>();

            // Registers the service for "Order" - service/repository

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Registers the service for "HttpClientWrapper" - service

            services.AddScoped<IHttpClientService, HttpClientWrapperService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // All the middleware in the application can be configured in this method
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
