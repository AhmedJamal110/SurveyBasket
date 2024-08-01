using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.ApI.Contracts.Authentication;
using SurveyBasket.ApI.Errors;
using SurveyBasket.ApI.Helper;
using SurveyBasket.ApI.JwtService;
using System.Security.Cryptography;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.ApI.Services
{
    public class AuthServices : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthServices> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _refreshTokenExpirationDays = 14;

        public AuthServices(UserManager<AppUser> userManager,  IJwtProvider jwtProvider , 
            SignInManager<AppUser> signInManager , ILogger<AuthServices> logger , IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }


       public async Task<Result> ResgisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
            };

           var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                 var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Confairmation Code {code} ", code);

                var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

               var emailBody =  EmailBodyBuilder.GenerateEmailBody("EmailConfigration", 
                    new Dictionary<string, string>
                    {
                        { "{{name}}" , user.FirstName },
                        {"{{action_url}}" ,  $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
                    });


                await _emailSender.SendEmailAsync(user.Email, "Confairmtion Email (Survey Basket)", emailBody);

            return Result.Success();
            
            }

            var error = result.Errors.First();

            return Result.Faliure<AuthResponse>(new Error(error.Code, error.Description));
        
        }



        public async Task<TResult<AuthResponse>>  GetTokenForUser(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Faliure<AuthResponse>(UserError.InvalidCredential);
            //return UserError.InvalidCredential;

             var result = await _signInManager.PasswordSignInAsync(user, password, false , false);
            if (result.Succeeded)
            {
                var (token, expireIn) = await _jwtProvider.CreateTokenAsync(user);

                var refeshToken = GenerateRefreshToken();
                var rereshTokenExpire = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

                var rseponse = new AuthResponse
                (user.Id, user.FirstName, user.LastName, user.Email, token, expireIn, refeshToken, rereshTokenExpire);

                user.RefreshTokens.Add(new RefreshTokens
                {
                    Token = refeshToken,
                    ExpiresOn = rereshTokenExpire
                });

                await _userManager.UpdateAsync(user);
                return Result.Success(rseponse);

            }
            return result.IsNotAllowed ? Result.Faliure<AuthResponse>(UserError.ConfirmEmail)
            : Result.Faliure<AuthResponse>(UserError.InvalidCredential);




            // return rseponse;
        }

        public async Task<AuthResponse?> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
        {
            var userId = _jwtProvider.ValidatToken(token);
            if (userId is null)
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return null;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newToken, expireIn) = await _jwtProvider.CreateTokenAsync(user);
            var newrefreshToken = GenerateRefreshToken();
            var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = newrefreshToken,
                ExpiresOn = newRefreshTokenExpiration,
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, newToken, expireIn, newrefreshToken, newRefreshTokenExpiration);

        }


        public async Task<bool> RevokeRefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
        {
           var userId =  _jwtProvider.ValidatToken(token);
            if (userId is null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(X => X.Token == refreshToken && X.IsActive);
            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return true;

        }






        public async Task<Result> ConfairmEmailAsync(ConfairmEmailRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
               // return Result.Failure(UserError.InvalidCodeOrToken);
               
                //   even email not found or userId is not vaild
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserError.DuplicatedEmailConfairm);

            var code = request.Code;
            try
            {
                 code =  Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserError.InvalidCodeOrToken);
            }


            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description));

        }



        public async Task<Result> ResendConfairmEmailAsync(ResendConfirmEmail request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserError.DuplicatedEmailConfairm);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Cnfairmation code", code);

            return Result.Success();
        }


        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

   
    }

}
