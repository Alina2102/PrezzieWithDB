namespace PrezzieWithDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        souvenirID = c.Int(nullable: false),
                        userName = c.String(nullable: false, maxLength: 128),
                        amount = c.Int(nullable: false),
                        reward = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.souvenirID)
                .ForeignKey("dbo.Customers", t => t.userName, cascadeDelete: true)
                .ForeignKey("dbo.Souvenirs", t => t.souvenirID)
                .Index(t => t.souvenirID)
                .Index(t => t.userName);
            
            AddColumn("dbo.Customers", "loggedIn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "souvenirID", "dbo.Souvenirs");
            DropForeignKey("dbo.Requests", "userName", "dbo.Customers");
            DropIndex("dbo.Requests", new[] { "userName" });
            DropIndex("dbo.Requests", new[] { "souvenirID" });
            DropColumn("dbo.Customers", "loggedIn");
            DropTable("dbo.Requests");
        }
    }
}
