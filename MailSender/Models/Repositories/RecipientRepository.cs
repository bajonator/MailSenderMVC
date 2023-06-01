using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MailSender.Models.Repositories
{
    public class RecipientRepository
    {
        public List<Recipient> GetRecipients(string userId)
        {
            using (var context = new ApplicationDbContext())
                return context.Recipients
                    .Where(x => x.UserId == userId)
                    .GroupBy(x => x.EmailAddress)
                    .Select(g => g.FirstOrDefault())                    
                    .ToList();
        }

        public void AddRecipient(string addressEmail, string userId)
        {

            using (var context = new ApplicationDbContext())
            {
                var recipient = new Recipient { EmailAddress = addressEmail, UserId = userId };
                context.Recipients.Add(recipient);
                context.SaveChanges();
            }
        }

        internal bool EmailExists(string emailAdress)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Recipients.Any(x => x.EmailAddress == emailAdress);
            }
        }
    }
}