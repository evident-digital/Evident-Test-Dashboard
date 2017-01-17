namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFailedFirstBuildIDColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestOccurrence", "FailedFirstBuildId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestOccurrence", "FailedFirstBuildId");
        }
    }
}
