namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Emails", "Recipient_Id", "dbo.Recipients");
            DropIndex("dbo.Emails", new[] { "Recipient_Id" });
            CreateTable(
                "dbo.RecipientEmails",
                c => new
                    {
                        Recipient_Id = c.Int(nullable: false),
                        Email_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipient_Id, t.Email_Id })
                .ForeignKey("dbo.Recipients", t => t.Recipient_Id, cascadeDelete: true)
                .ForeignKey("dbo.Emails", t => t.Email_Id, cascadeDelete: true)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Email_Id);
            
            AddColumn("dbo.Emails", "To", c => c.String(nullable: false));
            AddColumn("dbo.Emails", "EmailParams_Id", c => c.Int());
            CreateIndex("dbo.Emails", "EmailParams_Id");
            AddForeignKey("dbo.Emails", "EmailParams_Id", "dbo.EmailParams", "Id");
            DropColumn("dbo.Emails", "Recipient");
            DropColumn("dbo.Emails", "Recipient_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Emails", "Recipient_Id", c => c.Int());
            AddColumn("dbo.Emails", "Recipient", c => c.String(nullable: false));
            DropForeignKey("dbo.RecipientEmails", "Email_Id", "dbo.Emails");
            DropForeignKey("dbo.RecipientEmails", "Recipient_Id", "dbo.Recipients");
            DropForeignKey("dbo.Emails", "EmailParams_Id", "dbo.EmailParams");
            DropIndex("dbo.RecipientEmails", new[] { "Email_Id" });
            DropIndex("dbo.RecipientEmails", new[] { "Recipient_Id" });
            DropIndex("dbo.Emails", new[] { "EmailParams_Id" });
            DropColumn("dbo.Emails", "EmailParams_Id");
            DropColumn("dbo.Emails", "To");
            DropTable("dbo.RecipientEmails");
            CreateIndex("dbo.Emails", "Recipient_Id");
            AddForeignKey("dbo.Emails", "Recipient_Id", "dbo.Recipients", "Id");
        }
    }
}
