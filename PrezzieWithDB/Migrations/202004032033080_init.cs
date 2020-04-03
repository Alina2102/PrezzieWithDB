namespace PrezzieWithDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SouvenirInfoes",
                c => new
                    {
                        souvenirId = c.Int(nullable: false, identity: true),
                        price = c.Single(nullable: false),
                        description = c.String(),
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
            DropIndex("dbo.Souvenirs", new[] { "souvenirId" });
            DropTable("dbo.Souvenirs");
            DropTable("dbo.SouvenirInfoes");
        }
    }
}
