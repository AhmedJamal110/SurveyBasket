
using SurveyBasket.ApI.Extensions;

namespace SurveyBasket.ApI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services 
            builder.Services.AddServicesDependencies(builder.Configuration);

            //IdentityServices 
            builder.Services.AddIdentityServicesConfig(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
