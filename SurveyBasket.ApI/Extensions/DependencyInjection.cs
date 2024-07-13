
using Microsoft.OpenApi.Models;

namespace SurveyBasket.ApI.Extensions
{
    public  static class DependencyInjection
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddControllers();

            //SwaggerServices
            services.AddSwaggerServices();

            //dbContext
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IPollServices, PollServices>();
            //Mapster 

            services.AddMapsterConfigration();
            //fluentValidation

            services.AddFluentValidationConfigration();

            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(C =>
            {
                var SecuritySchema = new OpenApiSecurityScheme
                {
                    Name = "Authorizations",
                    Description = " Jwt Auth Bearer Schema",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,

                    }
                };

                C.AddSecurityDefinition("Bearer", SecuritySchema);
                var ScurityRequirments = new OpenApiSecurityRequirement
                {
                    {
                        SecuritySchema , new [] {"Bearer"}
                    }
                };

                C.AddSecurityRequirement(ScurityRequirments);
            });
            return services;



        }





        private static IServiceCollection AddMapsterConfigration(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(config));
            return services;
        }

        private static IServiceCollection AddFluentValidationConfigration(this IServiceCollection services)
        {

            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                 return services;
        }


    }
}
