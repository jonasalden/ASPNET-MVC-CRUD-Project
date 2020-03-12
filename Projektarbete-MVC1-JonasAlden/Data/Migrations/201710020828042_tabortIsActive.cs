namespace Projektarbete_MVC1_JonasAlden.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabortIsActive : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
