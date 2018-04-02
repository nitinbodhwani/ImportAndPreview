using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImportAndPreviewApi.Models
{
	public class SwipeInfo
	{
		public DateTime EventDate { get; set; }
		public string Location { get; set; }
		public string InTime { get; set; }
		public string OutTime { get; set; }
		public double TotalHours { get; set; }
	}
}