namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedCascadingDeleteConvention : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Build", "BuildTypeId", "dbo.BuildType");
            DropForeignKey("dbo.TestOccurrence", "BuildId", "dbo.Build");
            DropForeignKey("dbo.BuildType", "EnvironmentId", "dbo.Environment");
            DropForeignKey("dbo.Environment", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.TestOccurrence", "LabelId", "dbo.Label");
            AddColumn("dbo.Label", "DashboardId", c => c.Int(nullable: false));
            CreateIndex("dbo.Label", "DashboardId");
            AddForeignKey("dbo.Label", "DashboardId", "dbo.Dashboard", "DashboardId");
            AddForeignKey("dbo.Build", "BuildTypeId", "dbo.BuildType", "BuildTypeId");
            AddForeignKey("dbo.TestOccurrence", "BuildId", "dbo.Build", "BuildId");
            AddForeignKey("dbo.BuildType", "EnvironmentId", "dbo.Environment", "EnvironmentId");
            AddForeignKey("dbo.Environment", "DashboardId", "dbo.Dashboard", "DashboardId");
            AddForeignKey("dbo.TestOccurrence", "LabelId", "dbo.Label", "LabelId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestOccurrence", "LabelId", "dbo.Label");
            DropForeignKey("dbo.Environment", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.BuildType", "EnvironmentId", "dbo.Environment");
            DropForeignKey("dbo.TestOccurrence", "BuildId", "dbo.Build");
            DropForeignKey("dbo.Build", "BuildTypeId", "dbo.BuildType");
            DropForeignKey("dbo.Label", "DashboardId", "dbo.Dashboard");
            DropIndex("dbo.Label", new[] { "DashboardId" });
            DropColumn("dbo.Label", "DashboardId");
            AddForeignKey("dbo.TestOccurrence", "LabelId", "dbo.Label", "LabelId", cascadeDelete: true);
            AddForeignKey("dbo.Environment", "DashboardId", "dbo.Dashboard", "DashboardId", cascadeDelete: true);
            AddForeignKey("dbo.BuildType", "EnvironmentId", "dbo.Environment", "EnvironmentId", cascadeDelete: true);
            AddForeignKey("dbo.TestOccurrence", "BuildId", "dbo.Build", "BuildId", cascadeDelete: true);
            AddForeignKey("dbo.Build", "BuildTypeId", "dbo.BuildType", "BuildTypeId", cascadeDelete: true);
        }
    }
}
