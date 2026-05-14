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
        private const string AppPassword = "ydfepopuqoqsfklx";

        public async Task<bool> SendMovieShareEmail(string recipientEmail, string movieName, string trailerUrl, string genre)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(SenderEmail, AppPassword.Replace(" ", "")), // Ensures no spaces
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SenderEmail, "Hammer Premium Cinemas"),
                    Subject = $"🎬 Recommendation: {movieName}",
                    IsBodyHtml = true,
                };

                // Professional HTML Body
                mailMessage.Body = $@"
            <div style='background-color: #0B0E11; color: white; padding: 40px; font-family: sans-serif; border-radius: 15px;'>
                <h1 style='color: #00C896; margin-bottom: 10px;'>Hammer Premium Cinemas</h1>
                <p style='font-size: 16px;'>Your friend thinks you'd love a cinematic experience with <b>{movieName}</b>.</p>
                <hr style='border: 0; border-top: 1px solid #252A31; margin: 20px 0;'>
                <p><b>Genre:</b> {genre}</p>
                <p><b>Experience:</b> Immersive 4K & Dolby Atmos</p>
                <br>
                <a href='{trailerUrl}' style='background-color: #00C896; color: #0B0E11; padding: 12px 25px; text-decoration: none; font-weight: bold; border-radius: 5px;'>WATCH TRAILER</a>
                <br><br>
                <p style='font-size: 12px; color: #9BA1A6;'>This invitation was sent via the HammerMtheater Desktop App.</p>
            </div>";

                mailMessage.To.Add(recipientEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email failed: " + ex.Message);
                return false;
            }
        }
    }
}