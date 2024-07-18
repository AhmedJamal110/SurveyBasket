
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.ApI.Contracts.Polls;
using SurveyBasket.ApI.Extensions;

namespace SurveyBasket.ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize]
    public class PollsController : ControllerBase
    {
        private readonly IPollServices _pollServices;

        public PollsController(IPollServices pollServices)
        {
            _pollServices = pollServices;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PollResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var polls =  await _pollServices.GetALlPolls(cancellationToken);
            if (polls is null)
                return NotFound();

            var response = polls.Adapt<IEnumerable<PollResponse>>();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPoll(int id , CancellationToken cancellationToken)
        {
            var poll = await _pollServices.GetPollById(id, cancellationToken);

            return poll.IsSuccess
                ? Ok(poll.Value)
                : poll.ToProblem(StatusCodes.Status400BadRequest);
        
        }

        [HttpPost]
        public async Task<ActionResult<PollResponse>> CreatePoll(PollRequest request , CancellationToken cancellationToken)
        {
            var Issuccess = await _pollServices.AddPoll(request,cancellationToken);
            return Ok(Issuccess);
        
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePoll([FromRoute] int id  , [FromBody] PollRequest request , CancellationToken cancellationToken)
        {
           
           // var poll = request.Adapt<Poll>();
            
            var response = await  _pollServices.UpdatePoll( id , request , cancellationToken);

            return response.IsSuccess ?NoContent() : NotFound(response.Error); 
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePoll([FromRoute] int id , CancellationToken cancellationToken)
        {
            var isSuccess =  await _pollServices.DeletePoll(id, cancellationToken);

            if (!isSuccess)
                return NotFound("Poll Not Found");
            return Ok("Deleted Successfully");
        }


        [HttpPut("{id}/toggel")]
        public async Task<ActionResult> ToggelStatue (int id, CancellationToken cancellationToken)
        {
           var isSuccess = await  _pollServices.ToggelStatue(id, cancellationToken);

            if (!isSuccess)
                return NotFound();


            return Ok();
            
        }       



    }
}
