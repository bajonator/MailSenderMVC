namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Emails", "EmailParams_Id", "dbo.EmailParams");
            DropForeignKey("dbo.RecipientEmails", "Recipient_Id", "dbo.Recipients");
            DropForeignKey("dbo.RecipientEmails", "Email_Id", "dbo.Emails");
            DropIndex("dbo.Emails", new[] { "EmailParams_Id" });
            DropIndex("dbo.RecipientEmails", new[] { "Recipient_Id" });
            DropIndex("dbo.RecipientEmails", new[] { "Email_Id" });
            AddColumn("dbo.Recipients", "Email_Id", c => c.Int());
            CreateIndex("dbo.Recipients", "Email_Id");
            AddForeignKey("dbo.Recipients", "Email_Id", "dbo.Emails", "Id");
            DropColumn("dbo.Emails", "EmailParams_Id");
            DropColumn("dbo.Recipients", "EmailId");
            DropTable("dbo.RecipientEmails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RecipientEmails",
                c => new
                    {
                        Recipient_Id = c.Int(nullable: false),
                        Email_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipient_Id, t.Email_Id });
            
            AddColumn("dbo.Recipients", "EmailId", c => c.Int(nullable: false));
            AddColumn("dbo.Emails", "EmailParams_Id", c => c.Int());
            DropForeignKey("dbo.Recipients", "Email_Id", "dbo.Emails");
            DropIndex("dbo.Recipients", new[] { "Email_Id" });
            DropColumn("dbo.Recipients", "Email_Id");
            CreateIndex("dbo.RecipientEmails", "Email_Id");
            CreateIndex("dbo.RecipientEmails", "Recipient_Id");
            CreateIndex("dbo.Emails", "EmailParams_Id");
            AddForeignKey("dbo.RecipientEmails", "Email_Id", "dbo.Emails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RecipientEmails", "Recipient_Id", "dbo.Recipients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Emails", "EmailParams_Id", "dbo.EmailParams", "Id");
        }
    }
}
