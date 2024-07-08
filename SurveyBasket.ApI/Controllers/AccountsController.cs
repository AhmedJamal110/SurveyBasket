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
