namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Emails", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Emails", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Emails", "Body", c => c.String());
            AlterColumn("dbo.Emails", "Subject", c => c.String());
        }
    }
}
