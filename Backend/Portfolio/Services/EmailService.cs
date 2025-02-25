using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Portfolio.Modals;

namespace Portfolio.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() { }

        public void SendEmail(Email email)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("your mail", "password"),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("your mail", "Charan Vadla"),
                    Subject = email.Name,
                    Body = email.Message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email.EmailAddress);

                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {

            }
        }

        public Status SendConfirmationEmail(Email email)
        {
            if(email == null || string.IsNullOrEmpty(email.EmailAddress) || string.IsNullOrEmpty(email.Message))
            {
                return new Status { Success = false, Message = "Error sending email" };
            }
            else
            {
                try
                {
                    Email newMail = new Email()
                    {
                        EmailAddress = email.EmailAddress,
                        Message = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background: #007bff;
                    color: white;
                    padding: 15px;
                    text-align: center;
                    font-size: 20px;
                    font-weight: bold;
                    border-radius: 8px 8px 0 0;
                }}
                .content {{
                    padding: 20px;
                    font-size: 16px;
                    color: #333;
                    line-height: 1.6;
                }}
                .footer {{
                    margin-top: 20px;
                    padding-top: 15px;
                    border-top: 1px solid #ddd;
                    font-size: 14px;
                    color: #777;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>Sai Charan Vadla</div>
                <div class='content'>
                    <p>Hi {email.Name},</p>
                    <p>Thank you for visiting my website! This is an automated message to confirm that your inquiry has been received. I appreciate your time and will get back to you as soon as possible.</p>
                    <p>If your request is urgent, feel free to reach out to me directly at <strong>+49 15560735789</strong>.</p>
                    <p>Looking forward to connecting with you!</p>
                    <p>Best regards,</p>
                    <p><strong>Sai Charan Vadla</strong></p>
                </div>
                <div class='footer'>
                    © {DateTime.Now.Year} Sai Charan Vadla. All rights reserved.
                </div>
            </div>
        </body>
        </html>",
                        Name = "Thank You for Reaching Out!"
                    };


                    SendEmail(newMail);

                    Email newMail1 = new Email()
                    {
                        EmailAddress = "saicharanvadla2511@gmail.com",
                        Message = $"Hey, {email.Name} - {email.EmailAddress} has tried to contact you. This is the message  " + email.Message,
                        Name = email.Name
                    };

                    SendEmail(newMail1);
                    return new Status { Success = true };

                }
                catch (Exception ex)
                {
                    return new Status { Success = false, Message = ex.Message };

                    throw new Exception(ex.Message);
                }

            }
        }

    }
}
