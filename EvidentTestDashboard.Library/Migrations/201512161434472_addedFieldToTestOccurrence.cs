namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFieldToTestOccurrence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestOccurrence", "TestOccurrenceSucceeded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestOccurrence", "TestOccurrenceSucceeded");
        }
    }
}
