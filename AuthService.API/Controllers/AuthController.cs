using AuthService.API.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands;
using AuthService.Application.Registration;

namespace AuthService.API.Controllers
{
    [Route("campus-net/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register-start")]
        public async Task<IActionResult> Start(RegisterStartRequest r)
        {
            await _mediator.Send(new StartRegistrationCommand(r.Email, r.Name, r.PhoneNumber));
            return Ok();
        }

        [HttpPost("register-verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest r)
        {
            await _mediator.Send(new VerifyRegistrationOtpCommand(r.Email, r.Otp));
            return Ok();
        }

        [HttpPost("register-set-password")]
        public async Task<IActionResult> SetPassword(SetPasswordRequest r)
        {
            await _mediator.Send(new SetPasswordCommand(r.Email, r.Password));
            return Ok();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] LoginRequest r)
        {
            var token = await _mediator.Send(new LoginCommand(r.email, r.password));
            return Ok(token);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest req)
        {
            var token = await _mediator.Send(
                new RefreshTokenCommand(req.user,req.RefreshToken));

            return Ok(new { accessToken = token });
        }
        [HttpPost("logout")]    
        public async Task<IActionResult> Logout(LogOut logout)
        {
            await _mediator.Send(new LogOutCommand(logout.userId));
            return Ok();
        }

        [HttpPost("forgot-password-request")]
        public async Task<IActionResult> ForgotPasswordRequest(ForgotPasswordRequest request)
        {
            await _mediator.Send(new ForgotPasswordRequestCommand(request.Email));
            return Ok();
        }

        [HttpPost("forgot-password-opt-verification")]
        public async Task<IActionResult> ForgotPasswordOtpVerification(ForgotPasswordOtpVerification request)
        {
            await _mediator.Send(new PasswordOtpVerificationCommand(request.email,request.otp));
            return Ok();
        }

        [HttpPost("forgot-password-reset")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword request)
        {
            await _mediator.Send(new SetNewPasswordCommand(request.Email,request.NewPassword));
            return Ok();
        }

        [HttpPost("forgot-password-otp-resend")]
        public async Task<IActionResult> ResendOtpForPasswordChange(ResendOtpForPasswordChange request)
        {
            await _mediator.Send(new PasswordOtpResendCommand(request.email));
            return Ok();
        }
    }
}
