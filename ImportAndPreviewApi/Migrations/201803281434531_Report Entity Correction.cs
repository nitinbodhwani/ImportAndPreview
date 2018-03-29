namespace ImportAndPreviewApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportEntityCorrection : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ReportEntities", "Month");
            DropColumn("dbo.ReportEntities", "Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReportEntities", "Year", c => c.Int(nullable: true));
            AddColumn("dbo.ReportEntities", "Month", c => c.Int(nullable: true));
        }
    }
}
