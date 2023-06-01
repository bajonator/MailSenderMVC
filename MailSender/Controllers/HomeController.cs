using MailSender.Models.Domains;
using MailSender.Models.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;

namespace MailSender.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private EmailParamsRepository _emailParamsRepository = new EmailParamsRepository();
        private SendEmailRepository _sendEmailRepository = new SendEmailRepository();        
        private RecipientRepository _recipientRepository = new RecipientRepository();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var emailParam = _emailParamsRepository.GetEmailParams(userId);
            var email = new Email();
            email.History = _sendEmailRepository.GetEmails(userId);
            email.Recipient = _recipientRepository.GetRecipients(userId);

            if (emailParam == null)
                return View("EmailParams");
            else
            {               
                email.From = emailParam.SenderEmail;
                return View(email);
            }
        }

        public ActionResult EmailParams(int id = 0)
        {
            var userId = User.Identity.GetUserId();
            var emailParam = _emailParamsRepository.GetEmailParams(userId);
            return View(emailParam);
        }
        public ActionResult HistoryEmails(Email email)
        {
            var userId = User.Identity.GetUserId();
            email.History = _sendEmailRepository.GetEmails(userId);
            return View("", email.History);
        }
        public ActionResult Recipients(Email email)
        {
            var userId = User.Identity.GetUserId();
            email.Recipient = _recipientRepository.GetRecipients(userId);
            return PartialView("_ListRecipients", email.Recipient);
        }

        [HttpPost]
        public ActionResult EmailParams(EmailParams emailParams)
        {
            var userId = User.Identity.GetUserId();
            emailParams.UserId = userId;

            if (!ModelState.IsValid)
                return View("EmailParams", emailParams);

            var existingParams = _emailParamsRepository.GetEmailParams(userId);

            if (existingParams != null)
            {
                existingParams.HostSmtp = emailParams.HostSmtp;
                existingParams.Port = emailParams.Port;
                existingParams.SenderEmailPassword = emailParams.SenderEmailPassword;
                existingParams.SenderName = emailParams.SenderName;
                existingParams.SenderEmail = emailParams.SenderEmail;
                _emailParamsRepository.Update(emailParams);
            }
            else
                _emailParamsRepository.Add(emailParams);            

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendEmail(Email email)
        {
            var userId = User.Identity.GetUserId();
            var emailParams = _emailParamsRepository.GetEmailParams(userId);

            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
                return View("", email.Recipient);

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(emailParams.HostSmtp, emailParams.Port))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential(email.From, emailParams.SenderEmailPassword);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(email.From);

                    foreach (var recipient in email.To.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(recipient))
                        {
                            mailMessage.To.Add(recipient.Trim());
                            AddNewRecipient(recipient);
                        }
                    }
                    mailMessage.Subject = email.Subject;
                    mailMessage.Body = email.Body;
                    email.SentTime = DateTime.Now;
                    email.UserId = userId;

                    smtpClient.Send(mailMessage);
                }

                _sendEmailRepository.Add(email);
                email.History = _sendEmailRepository.GetEmails(userId);

                return Json(new { Success = true });

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message });
            }
        }

        private void AddNewRecipient(string recipient)
        {
            if (!_recipientRepository.EmailExists(recipient.Trim()))
            {             
                var userId = User.Identity.GetUserId();
                _recipientRepository.AddRecipient(recipient, userId);
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}