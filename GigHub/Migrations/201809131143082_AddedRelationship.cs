namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Relationships",
                c => new
                    {
                        FollowerId = c.String(nullable: false, maxLength: 128),
                        FolloweeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FollowerId, t.FolloweeId })
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FolloweeId)
                .Index(t => t.FollowerId)
                .Index(t => t.FolloweeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Relationships", "FolloweeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Relationships", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Relationships", new[] { "FolloweeId" });
            DropIndex("dbo.Relationships", new[] { "FollowerId" });
            DropTable("dbo.Relationships");
        }
    }
}
