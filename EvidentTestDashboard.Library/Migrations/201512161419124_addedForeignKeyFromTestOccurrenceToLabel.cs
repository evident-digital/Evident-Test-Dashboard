namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedForeignKeyFromTestOccurrenceToLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestOccurrence", "LabelId", c => c.Int(nullable: false));
            CreateIndex("dbo.TestOccurrence", "LabelId");
            AddForeignKey("dbo.TestOccurrence", "LabelId", "dbo.Label", "LabelId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestOccurrence", "LabelId", "dbo.Label");
            DropIndex("dbo.TestOccurrence", new[] { "LabelId" });
            DropColumn("dbo.TestOccurrence", "LabelId");
        }
    }
}
