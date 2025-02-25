using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Net.Mail;
using Portfolio.Services;
using Portfolio.Modals;


namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        public IEmailService _emailService;
        public PortfolioController(IEmailService emailService) 
        {
            _emailService = emailService;
        }


        [HttpGet("get-file/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            // Define the folder where files are stored
            var root = Path.Combine(Directory.GetCurrentDirectory(), "FileUploads");
            var filePath = Path.Combine(root, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {   
                return NotFound("File not found");
            }

            // Get the MIME type of the file
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream"; // Default MIME type
            }

            // Return the file
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName); // Inline display or download
        }

        [HttpPost("SendEmail")]
        public Status SendEmail(Email email)
        {

         return _emailService.SendConfirmationEmail(email);

        }



    }
}
