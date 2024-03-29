using System.Net.Mail;
using System.Net;
using ClothX.DbModels;

namespace ClothX.Services
{
	// Service for sending emails using SmtpClient
	public class MailSenderService
	{
		private static MailSenderService _instance;
		private static SmtpClient smtpClient;

		private static string emailAddress = "unoor2398@gmail.com";
		private static string password = "";  

		// Singleton instance of the MailSenderService
		public static MailSenderService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MailSenderService();
					smtpClient = new SmtpClient();
					smtpClient.Port = 587;
					smtpClient.Host = "smtp.gmail.com";
					smtpClient.EnableSsl = true;
					smtpClient.Timeout = 30000;
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtpClient.UseDefaultCredentials = false;
					smtpClient.Credentials = new NetworkCredential(emailAddress, password);
				}
				return _instance;
			}
		}

		private MailSenderService() { }

		// Send a test email
		public async Task TestEmailSend()
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(emailAddress);
			mail.To.Add(emailAddress);
			mail.Subject = "This is used for Testing Email Sending Services";
			mail.IsBodyHtml = true;

			string content = "Testing Completing";

			mail.Body = content;
			await smtpClient.SendMailAsync(mail);
		}

		// Send registration confirmation email to the user
		public async Task SendMailToUserOnRegister(string Email, string Name, string code)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(emailAddress);
			mail.To.Add(Email);
			mail.Subject = "Registration on ClothX";
			mail.IsBodyHtml = true;

			string content = "";

			mail.Body = content;
			//smtpClient.Send(mail);
		}

		// Resend email confirmation to the user
		public async Task ResendEmailConfirmation(string Email, string Name, string code)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(emailAddress);
			mail.To.Add(Email);
			mail.Subject = "Registration on ClothX";
			mail.IsBodyHtml = true;

			string content = "";

			mail.Body = content;
			//smtpClient.Send(mail);
		}

		// Send password reset token email to the user
		public async Task SendPasswordResetToken(string Email, string Name, string code)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(emailAddress);
			mail.To.Add(Email);
			mail.Subject = "Registration on ClothX";
			mail.IsBodyHtml = true;

			string content = "";

			mail.Body = content;
			//smtpClient.Send(mail);
		}

		// Send an email notification in response to feedback
		public async Task SendEmailOnFeedbackResponse(Review review)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(emailAddress);
			mail.To.Add(review.User.User.Email);
			mail.Subject = "Feedback Reviewed";
			mail.IsBodyHtml = true;

			string content = "";

			mail.Body = content;
			//smtpClient.Send(mail);
		}
	}
}