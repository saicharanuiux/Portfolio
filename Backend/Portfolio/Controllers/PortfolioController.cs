using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Portfolio.Services;
using Portfolio.Modals;
using Portfolio.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        public IEmailService _emailService;
        public IAuthService _authService;
        public IConfiguration _configuration;
        public ICurrentUserService _currentUserService;
        public PortfolioController(IEmailService emailService, IAuthService authService, IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _emailService = emailService;
            _authService = authService;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("file/{fileType}/{fileName}")]
        public async Task<IActionResult> GetFile(string fileType, string fileName)
        {
            string storageType = _configuration.GetValue<string>("fileStorage");

            if (storageType == "drive")
            {
                string userId = _currentUserService.GetUserId().ToString();

                byte[] fileBytes = await _authService.ReadFileFromDrive(userId, fileName);

                return File(fileBytes, fileType.ToLower() == "pdf" ? "application/pdf" : "application/octet-stream", fileType.ToLower() == "pdf" ? "Charan Vadla Full Stack.pdf" : "Profile.jpeg");
            }
            else if (storageType == "local")
            {
                var root = Path.Combine(Directory.GetCurrentDirectory(), "FileUploads");
                var filePath = Path.Combine(root, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found");
                }

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType, fileName);
            }
            else
            {
                return BadRequest("Invalid file storage configuration");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("SendEmail")]
        public Status SendEmail(Email email)
        {
            return _emailService.SendConfirmationEmail(email);
        }
    }
}
