using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.Repositories
{
    public class SendEmailRepository
    {
        public List<Email> GetEmails(string userId)
        {
            using (var context = new ApplicationDbContext())
                return context.Emails.Where(x => x.UserId == userId).ToList();
        }

        public void Add(Email email)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Emails.Add(email);
                context.SaveChanges();
            }
        }
    }
}