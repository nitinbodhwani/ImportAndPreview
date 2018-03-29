namespace ImportAndPreviewApi.EntityModels
{
	using System.Data.Entity;

	public class AttendanceContext : DbContext
	{
		// Your context has been configured to use a 'ReportDataModel' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'ImportAndPreviewApi.EntityModels.ReportDataModel' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'ReportDataModel' 
		// connection string in the application configuration file.
		public AttendanceContext()
			: base("name=AttendanceDB")
		{
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<ReportDataEntity> ReportData { get; set; }
		public virtual DbSet<ReportEntity> Report { get; set; }
		public virtual DbSet<ReaderEntity> Reader { get; set; }
	}
}