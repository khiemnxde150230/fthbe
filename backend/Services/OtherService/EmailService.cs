using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;


using backend.Helper;
using backend.Models;
using backend.Helper;

namespace backend.Services.OtherService
{
    public class EmailService
    {
        private static EmailService instance;

        public static EmailService Instance
        {
            get { if (instance == null) instance = new EmailService(); return EmailService.instance; }
            private set { EmailService.instance = value; }
        }

        public async Task<bool> SendMail(string mail, int type, string fullname, string account, string password)
        {
            try
            {
                // With type == 1, create account
                // With type == 2, forgot password
                string _text = "";
                string subject = "";
                if (type == 1)
                {
                    _text = EmailHelper.Instance.RegisterMail(fullname, account, mail);
                    subject = "Đăng ký tài khoản FPTTicketHub - Xác nhận tài khoản";
                }
                if (type == 2)
                {
                    _text = EmailHelper.Instance.ForgotMail(fullname, account, password);
                    subject = "Hỗ trợ tài khoản FPTTicketHub - Quên mật khẩu";
                }

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("fpttickethub@gmail.com"));
                email.To.Add(MailboxAddress.Parse(mail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = _text
                };
                var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync("fpttickethub@gmail.com", "urjiyqjypwkfazec");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> SendEventReminderMail(string email, string fullname, string eventName, DateTime eventStartTime, string eventLocation, string eventAddress)
        {
            try
            {
                string subject = $"Lời nhắc: Sự kiện {eventName} sắp diễn ra";
                string _text = EmailHelper.Instance.EventReminderMail(fullname, eventName, eventStartTime, eventLocation, eventAddress);

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("fpttickethub@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = _text
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync("fpttickethub@gmail.com", "urjiyqjypwkfazec");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> SendRatingRequestMail(string email, string fullname, string eventName, int eventRatingId)
        {
            try
            {
                string subject = $"Đánh giá sự kiện {eventName}";
                string _text = EmailHelper.Instance.RatingRequestMail(fullname, eventName, eventRatingId);
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("fpttickethub@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = _text
                };
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync("fpttickethub@gmail.com", "urjiyqjypwkfazec");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendTicketEmail(string mail, string fullName, int ticketId, string ticketType, int quantity, string orderId, string paymentAmount, string eventName, DateTime eventStartTime, string eventLocation, string eventAddress)
        {
            try
            {
                string _text = "";
                string subject = "";
                _text = EmailHelper.Instance.TicketEmail(fullName, ticketId, ticketType, quantity, orderId, paymentAmount, eventName, eventStartTime, eventLocation, eventAddress);
                subject = $"FPTTicketHub - Vé điện tử cho đơn hàng {orderId}";


                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("fpttickethub@gmail.com"));
                email.To.Add(MailboxAddress.Parse(mail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = _text
                };
                var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync("fpttickethub@gmail.com", "urjiyqjypwkfazec");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> SendEventApproveEmail(string mail, int type, string fullname, string eventName)
        {
            try
            {
                // With type == 1, the first event by an account is approved, the account is become organizer
                // With type == 2, event approved
                string _text = "";
                string subject = "";
                if (type == 1)
                {
                    _text = EmailHelper.Instance.ApprovedEventFirstTimeEmail(mail, fullname, eventName);
                    subject = "FPTTicketHub - Bạn đã trở thành ban tổ chức";
                }
                if (type == 2)
                {
                    _text = EmailHelper.Instance.ApprovedEventEmail(mail, fullname, eventName);
                    subject = "FPTTicketHub - Sự kiện của bạn đã được duyệt";
                }

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("fpttickethub@gmail.com"));
                email.To.Add(MailboxAddress.Parse(mail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = _text
                };
                var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync("fpttickethub@gmail.com", "urjiyqjypwkfazec");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
