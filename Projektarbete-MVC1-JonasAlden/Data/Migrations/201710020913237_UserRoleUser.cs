namespace Projektarbete_MVC1_JonasAlden.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoleUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserRole");
        }
    }
}
