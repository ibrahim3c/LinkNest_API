using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Application.Identity.ForgetPassword;
using LinkNest.Application.Identity.Login;
using LinkNest.Application.Identity.RefreshToken;
using LinkNest.Application.Identity.Register;
using LinkNest.Application.Identity.ResetPassword;
using LinkNest.Application.Identity.RevokeToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V1.Accounts
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController:ControllerBase
    {
        private readonly ISender sender;

        public AccountsController(ISender sender)
        {
            this.sender = sender;
        }
        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var command = new LoginCommand(request.Email,request.Password);
            var result = await sender.Send(command);

            if(!string.IsNullOrEmpty(result.RefreshToken))
                 setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);

            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        // register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(request.Fname, request.Lname, request.CurrentCity, request.PhoneNumber, request.BirthDate, request.Email,request.Password, request.ConfirmPassword);
            var result = await sender.Send(command);
            if(!result.Success)
                return BadRequest(result);

            //set refresh token in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);
            return Ok(result);
        }


        private void setRefreshTokenInCookie(string refreshToken, DateTime refreshTokenExpiresOn)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiresOn.ToLocalTime()
            };

            HttpContext.Response.Cookies.Append(Constants.RefreshTokenKey, refreshToken, cookieOptions);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Cookies[Constants.RefreshTokenKey];

            var command = new RefreshTokenCommand(refreshToken);
            var result = await sender.Send(command);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPost("reset-token")]
        public async Task<IActionResult> RevokeToken(string? refreshToken)
        {
            string token = refreshToken ?? HttpContext.Request.Cookies[Constants.RefreshTokenKey];
            var command = new RevokeTokenCommand(token);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var command = new ForgetPasswordCommand(email);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

            // after that user click on link and go to frontend page that
            //1-capture userId, code
            //2-make form for user to reset new password
            // then user send data to reset password endpoint
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request.userId,request.code,request.newPassword);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

    }
}
