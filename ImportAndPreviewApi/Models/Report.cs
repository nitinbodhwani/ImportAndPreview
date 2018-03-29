using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImportAndPreviewApi.Models
{
	public class Report
	{
		/// <summary>
		/// Gets or sets Report Id
		/// </summary>
		public int ReportId { get; set; }

		/// <summary>
		/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets ImportDate
		/// </summary>
		public DateTime ImportDate { get; set; }

		/// <summary>
		/// Gets or sets Report Data
		/// </summary>
		public IList<ReportData> ReportDataList { get; set; }
	}
}