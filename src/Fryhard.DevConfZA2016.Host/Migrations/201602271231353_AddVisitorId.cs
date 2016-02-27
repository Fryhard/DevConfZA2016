namespace Fryhard.DevConfZA2016.Host.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVisitorId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "VoterId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "VoterId");
        }
    }
}
