using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.ApI.Extensions
{
    public static class ResultExtensions
    {
        public static ObjectResult  ToProblem (this Result result , int status  )
        {
            if (result.IsSuccess)
                throw new InvalidOperationException();

            var problem = Results.Problem(statusCode: status);

                // useing refelction 
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {
                    "errors" , new[] {result.Error}
                }
            };



            return new ObjectResult(problemDetails);
        
        }
        
    
    
    }
}
