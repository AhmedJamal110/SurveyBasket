using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.ApI.Contracts.Question;
using SurveyBasket.ApI.Errors;
using SurveyBasket.ApI.Extensions;

namespace SurveyBasket.ApI.Controllers
{
    [Authorize]
    [Route("api/polls/{pollId}[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionServices _questionServices;

        public QuestionsController(IQuestionServices questionServices)
        {
            _questionServices = questionServices;
        }

        [HttpPost]
        public async Task<ActionResult> AddQuestion([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionServices.AddQuestionAsync(pollId, request, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);

            return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent)
            ? result.ToProblem(StatusCodes.Status409Conflict)
            : result.ToProblem(StatusCodes.Status404NotFound);

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionServices.GetAllQuestions(pollId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status404NotFound);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById( int PolId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _questionServices.GetQustionById(PolId, id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status404NotFound);
        }


        [HttpPut("{id}/toggelAction")]
        public async Task<ActionResult> ToggelAction([FromRoute] int pollId , [FromRoute] int id , CancellationToken cancellationToken)
        {
            var result = await _questionServices.ToggelStatue(pollId, id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status404NotFound); 
        }
    
    
    }
}
