using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.ApI.Errors;
using SurveyBasket.ApI.Extensions;
using System.Security.Claims;

namespace SurveyBasket.ApI.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly IQuestionServices _questionServices;

        public VotesController(IQuestionServices  questionServices)
        {
            _questionServices = questionServices;
        }

        [HttpGet]
        public async Task<ActionResult> StartVote( [FromRoute] int pollId , CancellationToken cancellationToken )
        {
            var userId = User.GetUserID();

            var result = await _questionServices.GetAvailableQuestionAsync(pollId, userId, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return result.Error.Equals(VoteErrors.VoteDublicated)
                ? result.ToProblem(StatusCodes.Status409Conflict)
                : result.ToProblem(StatusCodes.Status404NotFound);
        }   

    }
}
