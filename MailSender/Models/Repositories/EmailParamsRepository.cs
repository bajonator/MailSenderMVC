using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.Repositories
{
    public class EmailParamsRepository
    {
        public EmailParams GetEmailParams(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.EmailParams.FirstOrDefault(x => x.UserId == userId);
            }
        }

        public void Add(EmailParams emailParams)
        {
            using (var context = new ApplicationDbContext())
            {
                context.EmailParams.Add(emailParams);
                context.SaveChanges();
            }
        }

        public void Update(EmailParams emailParams)
        {
            using (var context = new ApplicationDbContext())
            {
                var paramsToUpdate = context.EmailParams.FirstOrDefault(x => x.UserId == emailParams.UserId);

                paramsToUpdate.HostSmtp = emailParams.HostSmtp;
                paramsToUpdate.Port = emailParams.Port;
                paramsToUpdate.SenderEmailPassword = emailParams.SenderEmailPassword;
                paramsToUpdate.SenderName = emailParams.SenderName;
                paramsToUpdate.SenderEmail = emailParams.SenderEmail;

                context.SaveChanges();
            }
        }
    }
}