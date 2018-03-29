namespace ImportAndPreviewApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReaderEntities",
                c => new
                    {
                        ReaderId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.ReaderId);
            
            CreateTable(
                "dbo.ReportEntities",
                c => new
                    {
                        ReportId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        ImportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportId);
            
            CreateTable(
                "dbo.ReportDataEntities",
                c => new
                    {
                        ReportDataId = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Node = c.String(),
                        Panel = c.String(),
                        Event = c.String(),
                        EventDateTime = c.DateTime(nullable: false),
                        CardNumber = c.Int(nullable: false),
                        CardName = c.String(),
                        Location = c.String(),
                        ReaderId = c.Int(nullable: false),
                        In = c.String(),
                        Out = c.String(),
                        Affiliation = c.String(),
                        AlarmText = c.String(),
                    })
                .PrimaryKey(t => t.ReportDataId)
                .ForeignKey("dbo.ReaderEntities", t => t.ReaderId, cascadeDelete: true)
                .ForeignKey("dbo.ReportEntities", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId)
                .Index(t => t.ReaderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportDataEntities", "ReportId", "dbo.ReportEntities");
            DropForeignKey("dbo.ReportDataEntities", "ReaderId", "dbo.ReaderEntities");
            DropIndex("dbo.ReportDataEntities", new[] { "ReaderId" });
            DropIndex("dbo.ReportDataEntities", new[] { "ReportId" });
            DropTable("dbo.ReportDataEntities");
            DropTable("dbo.ReportEntities");
            DropTable("dbo.ReaderEntities");
        }
    }
}
