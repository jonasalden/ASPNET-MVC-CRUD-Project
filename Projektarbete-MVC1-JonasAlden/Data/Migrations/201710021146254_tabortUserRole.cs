namespace Projektarbete_MVC1_JonasAlden.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabortUserRole : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "UserRole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserRole", c => c.String());
        }
    }
}
