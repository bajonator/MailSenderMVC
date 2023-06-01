using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MailSender.Models.Domains
{
    public class EmailParams
    {
        public int Id { get; set; }
        [Required]
        public string HostSmtp { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        [Display(Name = "Adres nadawcy e-mail:")]
        public string SenderEmail { get; set; }
        [Required]
        [Display(Name = "Hasło:")]
        public string SenderEmailPassword { get; set; }
        [Required]
        [Display(Name = "Nazwa nadawcy e-mail:")]
        public string SenderName { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}