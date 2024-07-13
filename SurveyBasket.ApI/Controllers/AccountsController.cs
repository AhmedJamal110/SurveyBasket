using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.ApI.Contracts.Authentication;

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

        [HttpPost]
        public async Task<ActionResult> Login(AuthRequest request , CancellationToken cancellationToken)
        {
            var result = await _authService.GetTokenForUser(request.Email, request.Password, cancellationToken);


            return result is null ? BadRequest("Invalid Email or Password") : Ok(result);
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
        public async Task<ActionResult<AuthResponse>> Register(AuthRequest request)
        {
            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.Email.Split("@")[0]

            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest("Email Invalid");

            return Ok();

        }
    
    }
}
