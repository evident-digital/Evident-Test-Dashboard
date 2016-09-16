namespace EvidentTestDashboard.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Label_Order_column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Label", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Label", "Order");
        }
    }
}
