namespace PrezzieWithDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class description : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SouvenirInfoes", "descriptionSouv", c => c.String());
            DropColumn("dbo.SouvenirInfoes", "description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SouvenirInfoes", "description", c => c.String());
            DropColumn("dbo.SouvenirInfoes", "descriptionSouv");
        }
    }
}
