using System;

namespace ImportAndPreviewApi.Models
{
	public class ReportData
	{
		/// <summary>
		/// Gets or sets Report Data Id
		/// </summary>
		public int ReportDataId { get; set; }

		/// <summary>
		/// Gets or sets Report Id
		/// </summary>
		public int ReportId { get; set; }

		/// <summary>
		/// Gets or sets  Node
		/// </summary>
		public string Node { get; set; }

		/// <summary>
		/// Gets or sets Panel
		/// </summary>
		public string Panel { get; set; }

		/// <summary>
		/// Gets or sets Event
		/// </summary>
		public string Event { get; set; }

		/// <summary>
		/// Gets or sets EventDateTime
		/// </summary>
		public DateTime EventDateTime { get; set; }

		/// <summary>
		/// Gets or sets Card Number
		/// </summary>
		public int CardNumber { get; set; }

		/// <summary>
		/// Gets or sets Card Name
		/// </summary>
		public string CardName { get; set; }

		/// <summary>
		/// Gets or sets Location
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets Reader Id
		/// </summary>
		public int ReaderId { get; set; }

		/// <summary>
		/// Gets or sets In field
		/// </summary>
		public string In { get; set; }

		/// <summary>
		/// Gets or sets Out field
		/// </summary>
		public string Out { get; set; }

		/// <summary>
		/// Gets or sets Affiliation
		/// </summary>
		public string Affiliation { get; set; }

		/// <summary>
		/// Gets or sets Alarm Text
		/// </summary>
		public string AlarmText { get; set; }
	}
}