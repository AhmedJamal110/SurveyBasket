
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

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }


        public static IServiceCollection AddMapsterConfigration(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(config));
            return services;
        }

        public static IServiceCollection AddFluentValidationConfigration(this IServiceCollection services)
        {

            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                 return services;
        }


    }
}
