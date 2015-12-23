namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRegexFieldToLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Label", "Regex", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Label", "Regex");
        }
    }
}
