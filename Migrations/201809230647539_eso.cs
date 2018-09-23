namespace LoginRegisterDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eso : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserSites");
        }
    }
}
