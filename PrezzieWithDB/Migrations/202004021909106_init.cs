namespace PrezzieWithDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "userName", "dbo.Profiles");
            DropIndex("dbo.Customers", new[] { "userName" });
            DropTable("dbo.Profiles");
            DropTable("dbo.Customers");
        }
    }
}
