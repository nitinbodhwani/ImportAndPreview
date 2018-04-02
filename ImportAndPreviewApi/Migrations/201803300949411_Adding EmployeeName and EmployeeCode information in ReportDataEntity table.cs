namespace ImportAndPreviewApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingEmployeeNameandEmployeeCodeinformationinReportDataEntitytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportDataEntities", "EmployeeName", c => c.String());
            AddColumn("dbo.ReportDataEntities", "EmployeeCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportDataEntities", "EmployeeCode");
            DropColumn("dbo.ReportDataEntities", "EmployeeName");
        }
    }
}
