namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Build",
                c => new
                    {
                        BuildId = c.Int(nullable: false, identity: true),
                        WebUrl = c.String(),
                        BuildTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BuildId)
                .ForeignKey("dbo.BuildType", t => t.BuildTypeId, cascadeDelete: true)
                .Index(t => t.BuildTypeId);
            
            CreateTable(
                "dbo.BuildType",
                c => new
                    {
                        BuildTypeId = c.Int(nullable: false, identity: true),
                        BuildTypeName = c.String(),
                        EnvironmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BuildTypeId)
                .ForeignKey("dbo.Environment", t => t.EnvironmentId, cascadeDelete: true)
                .Index(t => t.EnvironmentId);
            
            CreateTable(
                "dbo.Environment",
                c => new
                    {
                        EnvironmentId = c.Int(nullable: false, identity: true),
                        EnvironmentName = c.String(),
                        DashboardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EnvironmentId)
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.Dashboard",
                c => new
                    {
                        DashboardId = c.Int(nullable: false, identity: true),
                        DashboardName = c.String(),
                    })
                .PrimaryKey(t => t.DashboardId);
            
            CreateTable(
                "dbo.TestOccurrence",
                c => new
                    {
                        TestOccurrenceId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        BuildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestOccurrenceId)
                .ForeignKey("dbo.Build", t => t.BuildId, cascadeDelete: true)
                .Index(t => t.BuildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestOccurrence", "BuildId", "dbo.Build");
            DropForeignKey("dbo.Environment", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.BuildType", "EnvironmentId", "dbo.Environment");
            DropForeignKey("dbo.Build", "BuildTypeId", "dbo.BuildType");
            DropIndex("dbo.TestOccurrence", new[] { "BuildId" });
            DropIndex("dbo.Environment", new[] { "DashboardId" });
            DropIndex("dbo.BuildType", new[] { "EnvironmentId" });
            DropIndex("dbo.Build", new[] { "BuildTypeId" });
            DropTable("dbo.TestOccurrence");
            DropTable("dbo.Dashboard");
            DropTable("dbo.Environment");
            DropTable("dbo.BuildType");
            DropTable("dbo.Build");
        }
    }
}
