using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImportAndPreviewApi.EntityModels
{
	public class ReportEntity
	{
		/// <summary>
		/// Gets or sets Report Id
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ReportId { get; set; }

		/// <summary>
		/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets ImportDate
		/// </summary>
		public DateTime ImportDate { get; set; }
	}
}