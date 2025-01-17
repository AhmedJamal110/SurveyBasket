﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.ApI.Contracts.Authentication;
using SurveyBasket.ApI.Extensions;

namespace SurveyBasket.ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountsController(IAuthService  authService , UserManager<AppUser> userManager , SignInManager<AppUser> signInManager  )
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(AuthRequest request , CancellationToken cancellationToken)
        {
            var result = await _authService.GetTokenForUser(request.Email, request.Password, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem(StatusCodes.Status400BadRequest); 
               

            //return result.Match<ActionResult>(
            //    result => Ok(result),
            //      error => Problem(
            //          statusCode: StatusCodes.Status400BadRequest,
            //          title: "Bad Request",
            //          extens                      
                      
            //          ));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken (RefrshTokenRequest request , CancellationToken cancellationToken)
        {
           var result  = await _authService.GetRefreshToken(request.Token, request.RefreshToken, cancellationToken);
            if (result is null)
                return BadRequest("invalid token");
            
            return Ok(result);
        
        }

        [HttpPut("revoke-refresh-token")]
        public async Task<ActionResult> RevokeRefreshTokenAsync(RefrshTokenRequest request , CancellationToken cancellationToken)
        {
            var isRevoke = await _authService.RevokeRefreshToken(request.Token, request.RefreshToken, cancellationToken);


            return isRevoke ? Ok() : BadRequest("Operation Failded"); 
        
        }







        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request , CancellationToken cancellationToken)
        {
           var result =  await  _authService.ResgisterAsync(request , cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status401Unauthorized);
        }




        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfairmEmail(ConfairmEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.ConfairmEmailAsync(request, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }


        [HttpPost("resend-confirm-email")]
        public async Task<ActionResult> ResendConfairmEmail(ResendConfirmEmail request, CancellationToken cancellationToken)
        {
            var result = await _authService.ResendConfairmEmailAsync(request, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }

    }
}
