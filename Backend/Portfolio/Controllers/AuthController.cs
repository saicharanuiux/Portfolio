using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Entities;
using Portfolio.IServices;
using Portfolio.Modals;
using Portfolio.Models;
using System.Security.Claims;

namespace Portfolio.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, IConfiguration configuration, ICurrentUserService currentUserService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            User user = await authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("User already exists.");
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            var token = await authService.LoginAsync(request);
            if (token == null)
            {
                return BadRequest("Invalid credentials.");
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
           await authService.Logout();
           return Ok("Logged out successfully.");
        }

        [Authorize]
        [HttpGet]
        public ActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public ActionResult AdminOnlyEndpoint()
        {
            return Ok("You are and admin!");
        }

        [HttpGet("google")]
        public ActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-callback"
            };

            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<ActionResult> GoogleCallback()
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync("External");

            string jwt = await authService.LoginWithGmail(result);

            if (string.IsNullOrEmpty(jwt))
            {
                return Unauthorized();
            }

            return Redirect($"{configuration.GetValue<string>("Host")}/oauth-success?token={jwt}");
        }

        [Authorize(Roles="Admin")]
        [HttpGet("getUserDetails")]
        public CurrentUser GetUserDetails()
        {
            return currentUserService.GetCurrentUser();
        }
    }
}
