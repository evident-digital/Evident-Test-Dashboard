namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expandedDataModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Build", "TotalTestsRun", c => c.Int(nullable: false));
            AddColumn("dbo.Build", "Passed", c => c.Int(nullable: false));
            AddColumn("dbo.Build", "Failed", c => c.Int(nullable: false));
            AddColumn("dbo.Build", "TeamCityBuildId", c => c.Long(nullable: false));
            AddColumn("dbo.Build", "RunAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Build", "Number", c => c.String());
            AddColumn("dbo.Build", "Status", c => c.String());
            AddColumn("dbo.Build", "State", c => c.String());
            AddColumn("dbo.Build", "Href", c => c.String());
            AddColumn("dbo.Build", "BuildSucceeded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Build", "BuildSucceeded");
            DropColumn("dbo.Build", "Href");
            DropColumn("dbo.Build", "State");
            DropColumn("dbo.Build", "Status");
            DropColumn("dbo.Build", "Number");
            DropColumn("dbo.Build", "RunAt");
            DropColumn("dbo.Build", "TeamCityBuildId");
            DropColumn("dbo.Build", "Failed");
            DropColumn("dbo.Build", "Passed");
            DropColumn("dbo.Build", "TotalTestsRun");
        }
    }
}
