
using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.ApI.Abstractions
{
    public record Error(string code, string desceiption)
    {
        public static readonly Error None = new(string.Empty, string.Empty);

   
    }
}
