namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredParamNameandValuecolumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildType", "RequiredParamName", c => c.String());
            AddColumn("dbo.BuildType", "RequiredParamValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuildType", "RequiredParamValue");
            DropColumn("dbo.BuildType", "RequiredParamName");
        }
    }
}
