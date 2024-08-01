using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.ApI.JwtService;

namespace SurveyBasket.ApI.Extensions
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityServicesConfig(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthServices>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddHttpContextAccessor();
            var settingsJwt = configuration.GetSection(JwtOption.SectionName).Get<JwtOption>();
        
            
            services.AddOptions<JwtOption>()
                        .BindConfiguration(JwtOption.SectionName)
                        .ValidateDataAnnotations()
                        .ValidateOnStart();


            //services.Configure<JwtOption>(configuration.GetSection(JwtOption.SectionName));
         
            
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settingsJwt?.key!)),
                        ValidateIssuer = true,
                        ValidIssuer = settingsJwt?.ValidIssuer ,
                        ValidateAudience = true,
                        ValidAudience = settingsJwt?.ValidAudiance,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.User.RequireUniqueEmail = true;
            });


            return services;
        }
    }
}
