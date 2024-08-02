using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.ApI.Contracts.Profile;
using SurveyBasket.ApI.Extensions;
using System.Security.Claims;

namespace SurveyBasket.ApI.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfilesController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> Info()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _userService.GetProfileAsync(userId);
            return Ok(result.Value);

        }


        [HttpPut("edit-profile")]
        public async Task<ActionResult> UpdateProfile( [FromBody] UpdateProfileRequest request)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _userService.UpdateProfileAsync(userId, request);
           
            return NoContent();
        
        }


        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           var result =  await _userService.ChangePasswordAsync(userId , request);

            return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);
                

        }

    }
}
