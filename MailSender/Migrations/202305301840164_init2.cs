namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Emails", "Body", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Emails", "Body", c => c.String(nullable: false));
        }
    }
}
