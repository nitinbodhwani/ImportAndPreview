using System.Collections.Generic;

namespace ImportAndPreviewApi.Models
{
	public class EmployeeAttendance
	{
		public string EmployeeName { get; set; }
		public string EmployeeCode { get; set; }
		public double AggregatedHours { get; set; }
		public int MEP { get; set; }
		public int TDC { get; set; }
		public IList<SwipeInfo> SwipeInfoCollection { get; set; }
	}
}