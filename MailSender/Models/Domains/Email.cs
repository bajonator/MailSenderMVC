using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MailSender.Models.Domains
{
    public class Email
    {
        public Email()
        {
            History = new Collection<Email>();
            Recipient = new Collection<Recipient>();
        }
        public int Id { get; set; }
        [Display(Name = "Nadawca:")]
        public string From { get; set; }
        [Required(ErrorMessage = "Pole odbiorca jest wymagane")]
        [Display(Name = "Odbiorcy:")]
        public string To { get; set; }
        [Display(Name = "Temat:")]
        [Required(ErrorMessage = "Pole temat jest wymagane")]
        public string Subject { get; set; }
        [Display(Name = "Treść e-mail:")]
        [Required(ErrorMessage = "Pole treść jest wymagane")]
        public string Body { get; set; }
        public DateTime SentTime { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }


        public ApplicationUser User { get; set; }
        public ICollection<Email> History { get; set; }
        public ICollection<Recipient> Recipient { get; set; }
    }
}