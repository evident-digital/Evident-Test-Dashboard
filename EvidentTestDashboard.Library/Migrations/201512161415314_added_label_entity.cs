namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_label_entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Label",
                c => new
                    {
                        LabelId = c.Int(nullable: false, identity: true),
                        LabelName = c.String(),
                    })
                .PrimaryKey(t => t.LabelId);
            
            AddColumn("dbo.TestOccurrence", "TeamCityTestOccurrenceId", c => c.String());
            AddColumn("dbo.TestOccurrence", "Status", c => c.String());
            AddColumn("dbo.TestOccurrence", "Href", c => c.String());
            AddColumn("dbo.TestOccurrence", "Duration", c => c.Int());
            AddColumn("dbo.TestOccurrence", "Details", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestOccurrence", "Details");
            DropColumn("dbo.TestOccurrence", "Duration");
            DropColumn("dbo.TestOccurrence", "Href");
            DropColumn("dbo.TestOccurrence", "Status");
            DropColumn("dbo.TestOccurrence", "TeamCityTestOccurrenceId");
            DropTable("dbo.Label");
        }
    }
}
