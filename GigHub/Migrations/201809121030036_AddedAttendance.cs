namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAttendance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendeeId = c.String(nullable: false, maxLength: 128),
                        GigId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AttendeeId, t.GigId })
                .ForeignKey("dbo.AspNetUsers", t => t.AttendeeId, cascadeDelete: true)
                .ForeignKey("dbo.Gigs", t => t.GigId)
                .Index(t => t.AttendeeId)
                .Index(t => t.GigId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "GigId", "dbo.Gigs");
            DropForeignKey("dbo.Attendances", "AttendeeId", "dbo.AspNetUsers");
            DropIndex("dbo.Attendances", new[] { "GigId" });
            DropIndex("dbo.Attendances", new[] { "AttendeeId" });
            DropTable("dbo.Attendances");
        }
    }
}
