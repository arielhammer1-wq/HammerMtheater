using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HammerMtheater.Services
{
    public class EmailService
    {
        // Replace these with your actual email and an "App Password"
        private const string SenderEmail = "hammermtheater@gmail.com";
        private const string AppPassword = "your-app-password-here";

        public async Task<bool> SendMovieShareEmail(string recipientEmail, string movieName)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(SenderEmail, AppPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SenderEmail, "Hammer Premium Cinemas"),
                    Subject = $"🎬 Check out this movie: {movieName}",
                    Body = $"<h1>Experience the Future of Cinema</h1>" +
                           $"<p>Your friend thought you'd love <b>{movieName}</b> at Hammer Cinemas!</p>" +
                           $"<p>Book your tickets now in immersive 4K.</p>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipientEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine("Email failed: " + ex.Message);
                return false;
            }
        }
    }
}