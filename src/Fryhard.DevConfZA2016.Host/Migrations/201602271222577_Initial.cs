namespace Fryhard.DevConfZA2016.Host.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        UserAgent = c.String(),
                        IpAddress = c.String(),
                        ConnectionId = c.String(),
                        VoteValue = c.Int(nullable: false),
                        VoteDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VoteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Votes");
        }
    }
}
