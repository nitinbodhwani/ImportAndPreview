using System;

namespace ImportAndPreviewApi.Models
{
	public class ReportAggregate
	{
		public DateTime EventDate { get; set; }
		public int CardNumber { get; set; }
		public string CardName { get; set; }
		public string Location { get; set; }
		public string InTime { get; set; }
		public string OutTime { get; set; }
		public double TotalHours { get; set; }
	}
}