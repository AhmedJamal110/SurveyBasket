
using Hangfire;
using Microsoft.OpenApi.Models;
using SurveyBasket.ApI.Settings;

namespace SurveyBasket.ApI.Extensions
{
    public  static class DependencyInjection
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddControllers();
           
            //CorsOrigin
            var allowOrigin = configuration.GetSection("AllowedOrigins").Get<string[]>();
            services.AddCors(option =>
            {
                option.AddDefaultPolicy(opt =>
                {
                    opt
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(allowOrigin!);


                });
            });

            //SwaggerServices
            services.AddSwaggerDocumentationServices();

            //dbContext
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IPollServices, PollServices>();
            services.AddScoped<IQuestionServices, QuestionServices>();

            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));

            //Mapster 
             services.AddMapsterConfigration();
            
            //fluentValidation
             services.AddFluentValidationConfigration();

            //Hanfire
            services.AddHangfireJobsConfigration(configuration);
            return services;
        }



        private static IServiceCollection AddSwaggerDocumentationServices(this IServiceCollection services)
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
         private static IServiceCollection AddHangfireJobsConfigration(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                                            .UseSimpleAssemblyNameTypeSerializer()
                                                      .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));
            });

                      services.AddHangfireServer();
                          return services;
        }



    }
}
