using Microsoft.OpenApi.Models;

namespace SurveyBasket.ApI.Extensions
{
    public static class SwaggerDocuments
    {
        public static IServiceCollection AddSwaggerDocmuntation(this IServiceCollection services)
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

    }
}
