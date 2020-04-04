namespace PrezzieWithDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        countryUser = c.String(),
                    })
                .PrimaryKey(t => t.userName)
                .ForeignKey("dbo.Profiles", t => t.userName)
                .Index(t => t.userName);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        eMail = c.String(),
                        password = c.String(),
                        firstName = c.String(),
                        surname = c.String(),
                        birthday = c.String(),
                        descriptionUser = c.String(),
                    })
                .PrimaryKey(t => t.userName);
            
            CreateTable(
                "dbo.SouvenirInfoes",
                c => new
                    {
                        souvenirId = c.Int(nullable: false, identity: true),
                        price = c.Single(nullable: false),
                        descriptionSouv = c.String(),
                    })
                .PrimaryKey(t => t.souvenirId);
            
            CreateTable(
                "dbo.Souvenirs",
                c => new
                    {
                        souvenirId = c.Int(nullable: false),
                        souvenirName = c.String(),
                        countrySouv = c.String(),
                    })
                .PrimaryKey(t => t.souvenirId)
                .ForeignKey("dbo.SouvenirInfoes", t => t.souvenirId)
                .Index(t => t.souvenirId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Souvenirs", "souvenirId", "dbo.SouvenirInfoes");
            DropForeignKey("dbo.Customers", "userName", "dbo.Profiles");
            DropIndex("dbo.Souvenirs", new[] { "souvenirId" });
            DropIndex("dbo.Customers", new[] { "userName" });
            DropTable("dbo.Souvenirs");
            DropTable("dbo.SouvenirInfoes");
            DropTable("dbo.Profiles");
            DropTable("dbo.Customers");
        }
    }
}
