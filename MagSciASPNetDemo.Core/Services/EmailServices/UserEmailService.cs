using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.EmailServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Security.Claims;

namespace ContactsManagement.Core.Services.EmailServices
{
    public class UserEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserEmailService(IConfiguration configuration, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _config = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> SendEmail(string recipientEmail, string subject, string body)
        {
            // Retrieving email address of current logged in user

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userEmail = null;
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                   userEmail = await _userManager.GetEmailAsync(user);
                }
                else
                {
                    throw new AccessDeniedException();
                }
            }
            else
            {
                throw new AccessDeniedException();
            }


            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(userEmail));
            message.To.Add(MailboxAddress.Parse(recipientEmail));

            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            // Attaching files
            /*string filepath = "path/to/file.pdf";
            string fileExtension = Path.GetExtension(filepath);
            string filename = Path.GetFileName(filepath);

            var attachment = new MimePart("application", fileExtension)
            {
                Content = new MimeContent(File.OpenRead(filepath)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = $"{filename}.{fileExtension}"
            };*/
            //message.Body = new Multipart("mixed") { message.Body, attachment };

            try
            {
                using (var smtp = new SmtpClient())
                {
                    // Setup 2-Step Verification to be turned on in myaccount.google
                    // Generate App Password in myaccount.google : use that password
                    smtp.Connect(_config["EmailHost"], 587, SecureSocketOptions.StartTls);

                    smtp.Authenticate(_config["UserEmail"], _config["EMAILSERVICE_APPPASSWORD"]);

                    smtp.SendAsync(message).Wait();
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
